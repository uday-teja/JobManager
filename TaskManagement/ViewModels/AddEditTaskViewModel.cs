using System;
using System.Linq;
using System.Text;
using DataModel = TaskManagement.Data.Model;
using TaskManagement.Data.Service;
using TaskManagement.Data;
using AutoMapper;
using Caliburn.Micro;
using TaskManagement.Model.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helper = TaskManagement.Helper;
using UiModel = TaskManagement.Model.Model;


namespace TaskManagement.ViewModels
{
    public class AddEditTaskViewModel : BaseViewModel, IHandle<UiModel.Task>
    {
        private UiModel.Task task;

        public UiModel.Task Task
        {
            get { return task; }
            set
            {
                task = value;
                NotifyOfPropertyChange(() => Task);
            }
        }

        private string taskHeader = Helper.Constants.AddTaskHeader;

        public string TaskHeader
        {
            get { return taskHeader; }
            set
            {
                taskHeader = value;
                NotifyOfPropertyChange(() => TaskHeader);
            }
        }

        private string addEditButton = Helper.Constants.AddButton;

        public string AddEditButton
        {
            get { return addEditButton; }
            set
            {
                addEditButton = value;
                NotifyOfPropertyChange(() => AddEditButton);
            }
        }

        public UiModel.Task RawTask { get; set; }

        public AddEditTaskViewModel()
        {
            Task = new UiModel.Task();
        }

        public void AddEditTask()
        {
            Task.isDirty = true;
            if (Task.SelfValidate().IsValid)
            {
                if (Task.Id == Guid.Empty)
                {
                    eventAggregator.PublishOnCurrentThread(new KeyValuePair<TaskState, UiModel.Task>(TaskState.New, Task));
                }
                else
                {
                    eventAggregator.PublishOnCurrentThread(new KeyValuePair<TaskState, UiModel.Task>(TaskState.Update, Task));
                    TaskHeader = Helper.Constants.AddTaskHeader;
                    AddEditButton = Helper.Constants.AddButton;
                }
                Task = new UiModel.Task();
            }
            else
            {
                Task.Refresh();
            }
        }

        public void Cancel()
        {
            Task = new UiModel.Task();
            AddEditButton = Helper.Constants.AddButton;
            TaskHeader = Helper.Constants.AddTaskHeader;
        }

        public void Handle(UiModel.Task message)
        {
            TaskHeader = Helper.Constants.EditTaskHeader;
            AddEditButton = Helper.Constants.EditButton;
            Mapper.Map(message, Task);
        }
    }
}