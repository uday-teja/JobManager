using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Data.Model;
using System.IO;
using SQLite;
using DataModel = TaskManagement.Data.Model;
using TaskManagement.Data.IServices;
using AutoMapper;

namespace TaskManagement.Data.Service
{
    public class SqlLiteService : ITaskService
    {
        public string FileName;

        public SqlLiteService(string name)
        {
            FileName = name;
            var databasePath = Path.Combine(Environment.CurrentDirectory, FileName);
            var db = new SQLiteConnection(databasePath);
            db.CreateTable<DataModel.Task>();
        }

        public void Add(DataModel.Task task)
        {
            using (var db = new SQLiteConnection(FileName))
            {
                db.Insert(task);
            }
        }

        public void Delete(Guid id)
        {
            using (var db = new SQLiteConnection(FileName))
            {
                db.Execute("delete from Task where Id = ?", id);
            }
        }

        public DataModel.Task Get(Guid id)
        {
            using (var db = new SQLiteConnection(FileName))
            {
                DataModel.Task task = (from t in db.Table<DataModel.Task>() where t.ID == id select t).FirstOrDefault();
                return task;
            }
        }

        public List<DataModel.Task> GetAll()
        {
            List<DataModel.Task> models;
            using (var db = new SQLiteConnection(FileName))
            {
                models = (from t in db.Table<DataModel.Task>() select t).ToList();
            }
            return models;
        }

        public void Update(DataModel.Task task)
        {
            using (var db = new SQLiteConnection(FileName))
            {
                //var y = db.Table<DataModel.Task>().Where(x => x.ID == task.ID).FirstOrDefault();
                //Mapper.Map(task,y);
                db.Update(task);
            }
        }
    }
}