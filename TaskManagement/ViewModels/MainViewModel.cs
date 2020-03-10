using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace TaskManagement.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private AddEditTaskViewModel leftContainer;

        public AddEditTaskViewModel LeftContainer
        {
            get { return leftContainer; }
            set
            {
                leftContainer = value;
                NotifyOfPropertyChange(() => LeftContainer);
            }
        }

        private TaskManagerViewModel rightContainer;

        public TaskManagerViewModel RightContainer
        {
            get { return rightContainer; }
            set
            {
                rightContainer = value;
                NotifyOfPropertyChange(() => RightContainer);
            }
        }

        public MainViewModel()
        {
            LeftContainer = IoC.Get<AddEditTaskViewModel>();
            RightContainer = IoC.Get<TaskManagerViewModel>();
        }
    }
}