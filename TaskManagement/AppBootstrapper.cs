using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Notifications.Wpf;
using TaskManagement.Data.IServices;
using TaskManagement.ViewModels;

namespace TaskManagement
{
    public class AppBootstrapper : BootstrapperBase
    {
        SimpleContainer container;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            container = new SimpleContainer();
            BootstrapMapper.InitializeAutoMapper();
            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<ShellViewModel>();
            container.Singleton<MainViewModel>();
            container.Singleton<AddEditTaskViewModel>();
            container.Singleton<SettingsViewModel>();
            container.Singleton<TaskArchiveViewModel>();
            container.Singleton<TaskManagerViewModel>();
            container.Singleton<EventAggregator>();
            container.Singleton<INotificationManager, NotificationManager>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}