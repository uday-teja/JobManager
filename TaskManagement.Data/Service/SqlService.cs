using System;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;
using TaskManagement.Data.Model;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using DataModel = TaskManagement.Data.Model;
using TaskManagement.Model.Model;
using System.Windows;
using TaskManagement.Data.IServices;
using System.IO;

namespace TaskManagement.Data.Service
{
    public class SQLService : ITaskService
    {
        public string ConnectionString { get; set; }
        TaskContext TaskContext;
        public Connection Connection;
        public List<DataModel.Task> Tasks { get; set; }

        public SQLService(Connection connection)
        {
            Connection = new Connection();
            Connection = connection;
            ConnectionString = connection.FileName;
        }

        public void Authenticate()
        {
            switch (Connection.AuthenticationType)
            {
                case AuthenticationType.Windows:
                    WindowsAuthentication(Connection);
                    break;
                case AuthenticationType.SQLServer:
                    ServerAuthentication(Connection);
                    break;
            }
        }

        public void WindowsAuthentication(Connection connection)
        {
            ConnectionString = string.Format("data source=.;initial catalog={0};integrated security=True;", connection.FileName);
            CreateDatabase();
        }

        public void ServerAuthentication(Connection connection)
        {
            ConnectionString = string.Format("data source=.;initial catalog={0}; User Id={1};Password={2};", connection.FileName, connection.UserName, connection.Password);
            CreateDatabase();
        }

        public void CreateDatabase()
        {
            try
            {
                TaskContext = new TaskContext(ConnectionString);
                Tasks = GetAll();
            }
            catch
            {
                MessageBox.Show("Authentication Failed");
                File.Delete(Constants.UserSettings);
            }
        }

        public void Add(DataModel.Task task)
        {
            using (var ctx = new TaskContext(ConnectionString))
            {
                TaskContext.Tasks.AddOrUpdate(task);
                TaskContext.SaveChanges();
            }
        }

        public void Update(DataModel.Task task)
        {
            Add(task);
        }

        public void Delete(Guid id)
        {
            TaskContext.Tasks.Remove(TaskContext.Tasks.FirstOrDefault(t => t.ID == id));
            TaskContext.SaveChanges();
        }

        public DataModel.Task Get(Guid id)
        {
            return TaskContext.Tasks.FirstOrDefault(t => t.ID == id);
        }

        public List<DataModel.Task> GetAll()
        {
            using (var ctx = new TaskContext(ConnectionString))
            {
                return TaskContext.Tasks.ToList();
            }
        }
    }
}