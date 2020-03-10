using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using FluentValidation;
using FluentValidation.Results;

namespace TaskManagement.Model.Model
{
    public class Task : PropertyChangedBase, IDataErrorInfo
    {
        public AbstractValidator<Task> TaskValidator;

        private Guid id;

        public Guid Id
        {
            get { return id; }
            set { id = value; NotifyOfPropertyChange(() => Id); }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set
            { name = value; NotifyOfPropertyChange(() => Name); }
        }

        private DateTime startDate = DateTime.Now;

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; NotifyOfPropertyChange(() => StartDate); }
        }

        private DateTime dueDate = DateTime.Now;

        public DateTime DueDate
        {
            get { return dueDate; }
            set { dueDate = value; NotifyOfPropertyChange(() => DueDate); }
        }

        private Nullable<DateTime> completedDate = null;

        public Nullable<DateTime> CompletedDate
        {
            get { return completedDate; }
            set { completedDate = value; NotifyOfPropertyChange(() => CompletedDate); }
        }

        public int DaysLeft { get { return (DueDate - startDate).Days; } }

        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; NotifyOfPropertyChange(() => Description); }
        }

        private Status status;

        public Status Status
        {
            get { return status; }
            set { status = value; NotifyOfPropertyChange(() => Status); }
        }

        private Priority priority;

        public Priority Priority
        {
            get { return priority; }
            set { priority = value; NotifyOfPropertyChange(() => Priority); }
        }

        private static bool validateTask;

        public static bool ValidateTask
        {
            get { return validateTask; }
            set { validateTask = value; }
        }


        public string this[string columnName]
        {
            get
            {
                var errorDetails = SelfValidate();
                if (errorDetails != null)
                {
                    var firstOrDefault = errorDetails.Errors.FirstOrDefault(p => p.PropertyName == columnName);
                    if (firstOrDefault != null)
                        return TaskValidator != null ? firstOrDefault.ErrorMessage : "";
                }
                return "";
            }
        }

        public string Error
        {
            get
            {
                var results = SelfValidate();
                if (results != null && results.Errors.Any())
                {
                    var errors = string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage).ToArray());
                    return errors;
                }
                return string.Empty;
            }
        }
        public bool isDirty { get; set; }

        public ValidationResult SelfValidate()
        {
            return isDirty ? TaskValidator.Validate(this) : null;
        }

        public Task()
        {
            TaskValidator = new TaskValidator();
        }
    }
}