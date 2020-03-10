using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Data.IServices;

namespace TaskManagement.ViewModels
{
    public class BaseViewModel : Screen
    {
        public IEventAggregator eventAggregator;

        public BaseViewModel()
        {
            eventAggregator = IoC.Get<EventAggregator>();
            eventAggregator.Subscribe(this);
        }
    }
}