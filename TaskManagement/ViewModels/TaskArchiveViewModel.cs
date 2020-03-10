using System.Text;
using System.Threading.Tasks;
using TaskManagement.Data;
using TaskManagement.Data.Service;
using System.ComponentModel;
using DataModel = TaskManagement.Data.Model;
using System.Collections.Generic;
using UiModel = TaskManagement.Model.Model;
using TaskManagement.Data.IServices;
using System.Windows;
using AutoMapper;
using Caliburn.Micro;
using System;
using System.Linq;
using TaskManagement.Model.Model;

namespace TaskManagement.ViewModels
{
    public class TaskArchiveViewModel : BaseViewModel, IHandle<ITaskService>
    {
        public ITaskService taskService;

        private BindableCollection<UiModel.Task> tasks;

        public BindableCollection<UiModel.Task> Tasks
        {
            get { return tasks; }
            set
            {
                tasks = value;
                NotifyOfPropertyChange(() => Tasks);
            }
        }

        private DateTime fromDate = DateTime.Now;

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

        private Visibility noTasksFound = Visibility.Collapsed;
        public Visibility NoTasksFound
        {
            get { return noTasksFound; }
            set
            {
                noTasksFound = value;
                NotifyOfPropertyChange(() => NoTasksFound);
            }
        }

        private string pageInfo;

        public string PageInfo
        {
            get { return pageInfo; }
            set
            {
                pageInfo = value;
                NotifyOfPropertyChange(() => PageInfo);
            }
        }

        private UiModel.Task selectedTask;

        public UiModel.Task SelectedTask
        {
            get { return selectedTask; }
            set
            {
                selectedTask = value;
                NotifyOfPropertyChange(() => SelectedTask);
            }
        }

        private UiModel.Task editTask;

        public UiModel.Task EditTask
        {
            get { return editTask; }
            set
            {
                editTask = value;
                NotifyOfPropertyChange(() => EditTask);
            }
        }

        private bool isEditPopupOpen;

        public bool IsEditPopupOpen
        {
            get { return isEditPopupOpen; }
            set
            {
                isEditPopupOpen = value;
                NotifyOfPropertyChange(() => IsEditPopupOpen);
            }
        }

        private Visibility popupGridVisible = Visibility.Collapsed;

        public Visibility PopupGridVisible
        {
            get { return popupGridVisible; }
            set
            {
                popupGridVisible = value;
                NotifyOfPropertyChange(() => PopupGridVisible);
            }
        }

        public int PageCount;
        public List<UiModel.Task> RawTasks;
        public int TasksPerPage = 25;
        public int TotalPages;

        public void FirstPage()
        {
            if (PageCount > 1)
                Navigate(0, RawTasks);
        }

        public void PreviousPage()
        {
            if (PageCount > 1)
                Navigate(PageCount - 2, RawTasks);
        }

        public void NextPage()
        {
            if (PageCount < (TotalPages))
                Navigate(PageCount, RawTasks);
        }

        public void LastPage()
        {
            if (PageCount < TotalPages)
                Navigate(TotalPages - 1, RawTasks);
        }

        public void SearchByDate()
        {
            var Tasks = RawTasks.Where(d => d.StartDate >= FromDate.Date && d.StartDate <= ToDate.AddDays(1));
            Navigate(0, Tasks.ToList());
        }

        public void Navigate(int pageNo, List<UiModel.Task> tasks)
        {
            TotalPages = (int)Math.Ceiling((decimal)tasks.Count / TasksPerPage);
            if (tasks.Count >= TotalPages)
            {
                PageCount = pageNo + 1;
                Tasks = Mapper.Map<BindableCollection<UiModel.Task>>(tasks.Skip((pageNo) * TasksPerPage).Take(TasksPerPage));
                if (Tasks.Count == 0)
                    PageCount = 0;
                NoTasksFound = Tasks.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                SetPageNumber(PageCount, tasks);
            }
        }

        public void SetPageNumber(int pageInformation, List<UiModel.Task> employees)
        {
            PageInfo = PageCount + " of " + TotalPages;
        }

        public void EditSelectedTask()
        {
            EditTask = new UiModel.Task();
            Mapper.Map(SelectedTask, EditTask);
            IsEditPopupOpen = true;
            PopupGridVisible = Visibility.Visible;
        }

        public void CancelPopup()
        {
            IsEditPopupOpen = false;
            PopupGridVisible = Visibility.Collapsed;
        }

        public void UpdateTask()
        {
            EditTask.isDirty = true;
            if (EditTask.SelfValidate().IsValid)
            {
                UiModel.Task task = RawTasks.FirstOrDefault(t => t.Id == EditTask.Id);
                if (EditTask.Status == Status.Completed)
                    EditTask.CompletedDate = DateTime.Now;
                else
                    EditTask.CompletedDate = null;
                Mapper.Map(EditTask, task);
                eventAggregator.PublishOnCurrentThread(SelectedTask);
                eventAggregator.PublishOnCurrentThread(new KeyValuePair<TaskState, UiModel.Task>(TaskState.Update, EditTask));
                Navigate(0, RawTasks);
                IsEditPopupOpen = false;
                PopupGridVisible = Visibility.Collapsed;
            }
            else
            {
                EditTask.Refresh();
            }
        }

        public void SetTasks()
        {
            var Tasks = RawTasks.Where(d => d.StartDate >= FromDate && d.StartDate <= ToDate);
            Navigate(0, Tasks.ToList());
        }

        public void DisplayTasksArchieve()
        {
            eventAggregator.PublishOnUIThread(IoC.Get<TaskArchiveViewModel>());
            eventAggregator.PublishOnUIThread(taskService);
        }

        public void MainView()
        {
            eventAggregator.PublishOnUIThread(IoC.Get<MainViewModel>());
            eventAggregator.PublishOnUIThread(taskService);
        }

        public void Handle(ITaskService message)
        {
            taskService = message;
            RawTasks = Mapper.Map<List<UiModel.Task>>(taskService.GetAll());
            Navigate(0, RawTasks);
        }
    }
}