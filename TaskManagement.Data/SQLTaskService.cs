using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel = TaskManagement.Data.Model;
using System.Configuration;
using System.Data.SqlClient;

namespace TaskManagement.Data
{
    public class SQLTaskService : ITaskService
    {
        DataModel.TaskManagementEntities TaskManagementEntities;
        //static string conn = ConfigurationManager.ConnectionStrings["TaskManagementEntities"].ConnectionString;

        public SQLTaskService()
        {
            TaskManagementEntities = new DataModel.TaskManagementEntities();
        }

        public List<DataModel.Task> GetAll()
        {
            return TaskManagementEntities.Tasks.ToList();
        }

        public void Add(DataModel.Task task)
        {
            TaskManagementEntities.Tasks.AddOrUpdate(task);
            TaskManagementEntities.SaveChanges();
        }

        public void Update(DataModel.Task task)
        {
            DataModel.Task Update = GetAll().FirstOrDefault(e => e.ID == task.ID);
            Update = task;
            TaskManagementEntities.SaveChanges();
        }

        public void Delete(int id)
        {
            TaskManagementEntities.Tasks.Remove(TaskManagementEntities.Tasks.FirstOrDefault(t => t.ID == id));
            TaskManagementEntities.SaveChanges();
        }

        public DataModel.Task Get(int id)
        {
            return TaskManagementEntities.Tasks.FirstOrDefault(t => t.ID == id);
        }
    }
}