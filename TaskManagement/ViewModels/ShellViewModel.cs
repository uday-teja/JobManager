using Caliburn.Micro;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Resources;
using TaskManagement.Data.IServices;
using TaskManagement.Data.Model;
using TaskManagement.ViewModels;

namespace TaskManagement
{
    public class ShellViewModel : BaseViewModel, IHandle<DatabaseType>, IHandle<Screen>
    {
        private SettingsViewModel settings;

        public SettingsViewModel Settings
        {
            get { return settings; }
            set
            {
                settings = value;
                NotifyOfPropertyChange(() => Settings);
            }
        }

        private string selectedDatabaseName = Helper.Constants.NoDataBaseSelected;

        public string SelectedDatabaseName
        {
            get { return selectedDatabaseName; }
            set
            {
                selectedDatabaseName = value;
                NotifyOfPropertyChange(() => SelectedDatabaseName);
            }
        }

        private Screen mainWindowView;

        public Screen MainWindowView
        {
            get { return mainWindowView; }
            set
            {
                mainWindowView = value;
                NotifyOfPropertyChange(() => MainWindowView);
            }
        }

        public ITaskService TaskService { get; set; }

        public ShellViewModel()
        {
            MainWindowView = IoC.Get<MainViewModel>();
            Settings = IoC.Get<SettingsViewModel>();
            StreamResourceInfo sri = Application.GetResourceStream(new Uri("pack://application:,,,/Images/TaskManager_Tray.ico"));
            System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Icon = new System.Drawing.Icon(sri.Stream),
                Visible = true
            };
            notifyIcon.Click += NotifyIcon_Click;
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            var notifyIcon = (System.Windows.Forms.NotifyIcon)sender;
            var menu = new System.Windows.Forms.ContextMenu();
            var exitItem = new System.Windows.Forms.MenuItem { Name = "Exit", Text = "Exit" };
            exitItem.Click += ExitItem_Click;
            menu.MenuItems.Add(exitItem);
            notifyIcon.ContextMenu = menu;
            var mainWindow = Application.Current.Windows[0];
            mainWindow.Show();
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        public void OnClose(CancelEventArgs eventArgs)
        {
            eventArgs.Cancel = true;
            var result = Application.Current.Windows;
            Application.Current.Windows[0].Hide();
        }

        public void SettingsClick()
        {
            Settings.IsSetttingsVisible = true;
        }

        public void Handle(DatabaseType databaseType)
        {
            SelectedDatabaseName = databaseType.ToString();
        }

        public void Handle(Screen message)
        {
            MainWindowView = message;
        }
    }
}