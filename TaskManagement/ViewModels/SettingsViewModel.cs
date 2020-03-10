using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Data;
using Caliburn.Micro;
using TaskManagement.Data.Model;
using TaskManagement.Data.Service;
using TaskManagement.Data.IServices;
using TaskManagement.Model.Model;
using System.Windows;
using System.Security;

namespace TaskManagement.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public ConnectionService ConnectionService;

        private bool isSetttingsVisible;

        public bool IsSetttingsVisible
        {
            get { return isSetttingsVisible; }
            set
            {
                isSetttingsVisible = value;
                NotifyOfPropertyChange(() => IsSetttingsVisible);
            }
        }

        private Connection connection;

        public Connection Connection
        {
            get { return connection; }
            set
            {
                connection = value;
                NotifyOfPropertyChange(() => Connection);
            }
        }

        private int notifyTime = 60;

        public int NotifyTime
        {
            get { return notifyTime; }
            set
            {
                notifyTime = value;
                SetNotificationTime();
                NotifyOfPropertyChange(() => NotifyTime);
            }
        }

        public void SetNotificationTime()
        {
            eventAggregator.PublishOnUIThread(NotifyTime);
        }

        private bool isProcessRingActive;

        public bool IsProcessRingActive
        {
            get { return isProcessRingActive; }
            set
            {
                isProcessRingActive = value;
                NotifyOfPropertyChange(() => IsProcessRingActive);
            }
        }

        public SettingsViewModel()
        {
            Connection = new Connection();
            ConnectionService = new ConnectionService();
            Connection = ConnectionService.GetUserSettings();
            SetConnection();
        }

        public async void SetConnection()
        {
            eventAggregator.PublishOnUIThread(Connection.DatabaseType);
            if (!string.IsNullOrEmpty(Connection.FileName))
            {
                try
                {
                    IsProcessRingActive = true;
                    ConnectionService.SaveUserSettings(Connection);
                    switch (Connection.DatabaseType)
                    {
                        case DatabaseType.Json:
                            SetConnectionService(new JsonTaskService(Connection.FileName));
                            IsSetttingsVisible = false;
                            break;
                        case DatabaseType.SqlLite:
                            SetConnectionService(new SqlLiteService(Connection.FileName));
                            IsSetttingsVisible = false;
                            break;
                        case DatabaseType.SqlServer:
                            var sqlService = new SQLService(Connection);
                            try
                            {
                                await System.Threading.Tasks.Task.Run(() => sqlService.Authenticate());
                                SetConnectionService(sqlService);
                                IsProcessRingActive = false;
                                MessageBox.Show("Connected Successfully");
                                IsSetttingsVisible = false;
                            }
                            catch
                            {
                                IsSetttingsVisible = true;
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    ConnectionService.DeleteUserSettings();
                }
                finally
                {
                    IsProcessRingActive = false;
                }
            }
        }

        public void SetConnectionService(ITaskService taskService)
        {
            eventAggregator.PublishOnUIThread(taskService);
        }
    }
}