using AutoMapper;
using Caliburn.Micro;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using TaskManagement.Data.IServices;
using TaskManagement.Data.Service;
using TaskManagement.Model.Model;
using ToastNotifications;
using DataModel = TaskManagement.Data.Model;

namespace TaskManagement.ViewModels
{
    public class TaskManagerViewModel : BaseViewModel, IHandle<KeyValuePair<TaskState, Job>>, IHandle<Job>, IHandle<int>
    {

        private IJobService JobService;

        public List<Job> RawJobs { get; set; }

        private BindableCollection<Job> newJobs;

        public BindableCollection<Job> NewJobs
        {
            get { return newJobs; }
            set
            {
                newJobs = value;
                NotifyOfPropertyChange(() => NewJobs);
            }
        }

        private BindableCollection<Job> jobsInProgress;

        public BindableCollection<Job> JobsInProgress
        {
            get { return jobsInProgress; }
            set
            {
                jobsInProgress = value;
                NotifyOfPropertyChange(() => JobsInProgress);
            }
        }

        private BindableCollection<Job> jobsComplted;

        public BindableCollection<Job> JobsCompleted
        {
            get { return jobsComplted; }
            set
            {
                jobsComplted = value;
                NotifyOfPropertyChange(() => JobsCompleted);
            }
        }

        public Job selectedJob;

        public Job SelectedJob
        {
            get { return selectedJob; }
            set
            {
                selectedJob = value;
                NotifyOfPropertyChange(() => SelectedJob);
            }
        }

        private string dateDescription;

        public string DateDescription
        {
            get { return dateDescription; }
            set
            {
                dateDescription = value;
                NotifyOfPropertyChange(() => DateDescription);
            }
        }

        private DateTime fromDate = DateTime.Today.AddMonths(-1);

        public DateTime FromDate
        {
            get { return fromDate; }
            set
            {
                fromDate = value;
                NotifyOfPropertyChange(() => FromDate);
            }
        }

        private DateTime toDate = DateTime.Now;

        public DateTime ToDate
        {
            get { return toDate; }
            set
            {
                toDate = value;
                NotifyOfPropertyChange(() => ToDate);
            }
        }

        private Job rawJob;

        public Job RawJob
        {
            get { return rawJob; }
            set
            {
                rawJob = value;
                NotifyOfPropertyChange(() => RawJob);
            }
        }

        public TaskManagerViewModel(IJobService jobService)
        {
            JobService = new GoogleSheetsService(); ;
            if (JobService != null)
            {
                LoadTasks();
            }
        }

        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            var notificationManager = new NotificationManager();
            var items = RawJobs.Where(t => t.Status == "New" || t.Status == "In Progress");
            int count = 1;
            foreach (var item in items)
            {
                bool canShow = false;
                //if (item.DueDate.Date <= DateTime.Now.AddDays(3))
                //    canShow = true;
                //if (canShow)
                //{
                //    notificationManager.Show(new NotificationContent
                //    {
                //        Title = item.JobDetails,
                //        //Message = $"Due Date: {item.DueDate}",
                //        Message = GetNotificationMessage(item.DueDate),
                //        Type = TaskNotificataion(item.DueDate),

                //    }, "", TimeSpan.FromSeconds(count++ * 3));
                //}
            }
        }

        private string GetNotificationMessage(DateTime dueDate)
        {
            if (dueDate.Date.Date < DateTime.Now.Date)
                return $"You task is overdue - {dueDate.ToString("dddd, dd MMMM yyyy")}";
            else if (dueDate.Date.Date.ToShortDateString() == DateTime.Now.Date.ToShortDateString())
                return $"Your task is due today @{dueDate.ToString("hh:mm tt")}";
            else if (dueDate.Date.Date < DateTime.Now.AddDays(2).Date)
                return $"Your task is due tomorrow @{dueDate.ToString("hh:mm tt")}";
            else if (dueDate.Date.Date < DateTime.Now.AddDays(3).Date)
                return $"Your task to be completed by {dueDate.ToString("dddd, hh:mm tt")}";
            return string.Empty;
        }

        private NotificationType TaskNotificataion(DateTime dueDate)
        {
            if (dueDate.Date.Date < DateTime.Now.Date)
                return NotificationType.Error;
            else if (dueDate.Date.Date.ToShortDateString() == DateTime.Now.Date.ToShortDateString())
                return NotificationType.Success;
            else if (dueDate.Date.Date < DateTime.Now.AddDays(2).Date)
                return NotificationType.Warning;
            else if (dueDate.Date.Date < DateTime.Now.AddDays(3).Date)
                return NotificationType.Information;
            return NotificationType.Information;
        }

        public void LoadTasks()
        {
            RawJobs = JobService.GetAll();
            NewJobs = GetTasks("New");
            JobsInProgress = GetTasks("In Progress");
            JobsCompleted = GetTasks("Completed");
        }

        private BindableCollection<Job> GetTasks(string status)
        {
            return new BindableCollection<Job>(RawJobs.Where(t => t.Status == status));
        }

        public void TasksArcheiveView()
        {
            eventAggregator.BeginPublishOnUIThread(IoC.Get<TaskArchiveViewModel>());
            eventAggregator.BeginPublishOnUIThread(JobService);
        }

        public void CreateTask(Job job)
        {
            job.JobId = GetId();
            job.ActualCompletionDate = null;
            switch (job.Status)
            {
                case "New":
                    NewJobs.Add(job);
                    break;
                case "In Progress":
                    JobsInProgress.Add(job);
                    break;
                case "Completed":
                    job.ActualCompletionDate = DateTime.Now.ToString();
                    JobsCompleted.Add(job);
                    break;
            }
        }

        public void EditTask(Job task)
        {
            switch (task.Status)
            {
                case "New":
                    Job newTask = NewJobs.FirstOrDefault(t => t.JobId == task.JobId);
                    if (newTask != null)
                        Mapper.Map(task, newTask);
                    else
                    {
                        DeleteFromList(RawJob);
                        task.ActualCompletionDate = null;
                        NewJobs.Add(task);
                    }
                    break;
                case "In Progress":
                    Job inProgressTask = JobsInProgress.FirstOrDefault(t => t.JobId == task.JobId);
                    if (inProgressTask != null)
                        Mapper.Map(task, inProgressTask);
                    else
                    {
                        DeleteFromList(RawJob);
                        task.ActualCompletionDate = null;
                        JobsInProgress.Add(task);
                    }
                    break;
                case "Completed":
                    Job completedTask = JobsCompleted.FirstOrDefault(t => t.JobId == task.JobId);
                    if (completedTask != null)
                        Mapper.Map(task, completedTask);
                    else
                    {
                        DeleteFromList(RawJob);
                        task.ActualCompletionDate = DateTime.Now.ToString();
                        JobsCompleted.Add(task);
                    }
                    break;
            }
        }

        public void DeleteFromList(Job task)
        {
            switch (task.Status)
            {
                case "New":
                    NewJobs.Remove(NewJobs.FirstOrDefault(t => t.JobId == task.JobId));
                    break;
                case "In Progress":
                    JobsInProgress.Remove(JobsInProgress.FirstOrDefault(t => t.JobId == task.JobId));
                    break;
                case "Completed":
                    JobsCompleted.Remove(JobsCompleted.FirstOrDefault(t => t.JobId == task.JobId));
                    break;
            }
        }

        public void DragTask(EventArgs eventArgs)
        {
            if (SelectedJob != null)
            {
                MouseEventArgs mouseArgs = (MouseEventArgs)eventArgs;
                if (mouseArgs.LeftButton == MouseButtonState.Pressed)
                {
                    DataObject dragData = new DataObject(Helper.Constants.SelectedTask, SelectedJob);
                    DragDrop.DoDragDrop((DependencyObject)mouseArgs.Source, dragData, DragDropEffects.Move);
                }
            }
        }

        public void DropTask(DragEventArgs e)
        {
            string name = ((DependencyObject)e.Source).GetValue(FrameworkElement.NameProperty) as string;
            if (e.Data.GetDataPresent(Helper.Constants.SelectedTask))
            {
                Job job = e.Data.GetData(Helper.Constants.SelectedTask) as Job;
                DragAndDrop(name, job);
            }
        }

        public void DragAndDrop(String name, Job job)
        {
            switch (name)
            {
                case nameof(NewJobs):
                    if (job.Status != "New")
                    {
                        DeleteFromList(job);
                        job.Status = "New";
                        job.ActualCompletionDate = null;
                        NewJobs.Add(job);
                        JobService.Update(job);
                    }
                    break;
                case nameof(JobsInProgress):
                    if (job.Status != "In Progress")
                    {
                        DeleteFromList(job);
                        job.ActualCompletionDate = null;
                        job.Status = "In Progress";
                        JobsInProgress.Add(job);
                        JobService.Update(job);
                    }
                    break;
                case nameof(JobsCompleted):
                    if (job.Status != "Completed")
                    {
                        DeleteFromList(job);
                        job.ActualCompletionDate = DateTime.Now.ToString();
                        job.Status = "Completed";
                        JobsCompleted.Add(job);
                        JobService.Update(job);
                    }
                    break;
            }
        }

        public void DeleteTask()
        {
            if (MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                DeleteTask(SelectedJob);
            }
        }

        public void StartTimer()
        {
            this.SelectedJob.Stopwatch = new System.Diagnostics.Stopwatch();
            this.SelectedJob.Stopwatch.Start();
        }

        public void StopTimer()
        {
            this.SelectedJob.Stopwatch.Stop();
            TimeSpan ts = this.SelectedJob.Stopwatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
            this.SelectedJob.TotalTime = elapsedTime;
        }

        public void Handle(IJobService message)
        {
            JobService = message;
            t.Stop();
            t.Elapsed -= T_Elapsed;
            t.Elapsed += T_Elapsed;
            t.Start();
            LoadTasks();
        }

        public void Handle(KeyValuePair<TaskState, Job> message)
        {
            switch (message.Key)
            {
                case TaskState.New:
                    CreateTask(message.Value);
                    JobService.Add(message.Value);
                    break;
                case TaskState.Update:
                    EditTask(message.Value);
                    JobService.Update(message.Value);
                    break;
            }
        }

        public void DeleteTask(Job task)
        {
            switch (task.Status)
            {
                case "New":
                    NewJobs.Remove(NewJobs.FirstOrDefault(t => t.JobId == task.JobId));
                    JobService.Delete(task.JobId);
                    break;
                case "In Progress":
                    JobsInProgress.Remove(JobsInProgress.FirstOrDefault(t => t.JobId == task.JobId));
                    JobService.Delete(task.JobId);
                    break;
                case "Completed":
                    JobsCompleted.Remove(JobsCompleted.FirstOrDefault(t => t.JobId == task.JobId));
                    JobService.Delete(task.JobId);
                    break;
            }
        }

        public void SetEditTask()
        {
            RawJob = SelectedJob;
            eventAggregator.BeginPublishOnUIThread(RawJob);
        }

        public string GetId()
        {
            return Guid.NewGuid().ToString();
        }

        public void Handle(Job message)
        {
            RawJob = message;
        }

        public void DisplayTasksArchieve()
        {
            eventAggregator.PublishOnUIThread(IoC.Get<TaskArchiveViewModel>());
            eventAggregator.PublishOnUIThread(JobService);
        }

        public void MainView()
        {
            eventAggregator.PublishOnUIThread(IoC.Get<MainViewModel>());
            eventAggregator.PublishOnUIThread(JobService);
        }

        public Notifier Notifier;

        readonly Timer t = new Timer(1000 * 3600);

        public void Handle(int message)
        {
            t.Interval = 1000 * 60 * message;
        }
    }
}