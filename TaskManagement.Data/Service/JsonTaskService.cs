using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Data.Model;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using DataModel = TaskManagement.Data.Model;
using AutoMapper;
using TaskManagement.Data.IServices;

namespace TaskManagement.Data.Service
{
    public class JsonTaskService : ITaskService
    {
        public string JsonFile;
        public string JsonData;
        public List<DataModel.Task> Tasks { get; set; }

        public JsonTaskService(string str)
        {
            JsonFile = str + ".json";
            Tasks = GetAll();
        }

        public void Add(DataModel.Task task)
        {
            Tasks.Add(task);
            WriteData(Tasks, JsonData);
        }

        public void Update(DataModel.Task task)
        {
            DataModel.Task updateTask = Tasks.FirstOrDefault(t => t.ID == task.ID);
            Mapper.Map(task, updateTask);
            WriteData(Tasks, JsonData);
        }

        public void Delete(Guid id)
        {
            Tasks.Remove(Tasks.FirstOrDefault(t => t.ID == id));
            WriteData(Tasks, JsonData);
        }

        public DataModel.Task Get(Guid id)
        {
            return (DataModel.Task)Tasks.Where(t => t.ID == id);
        }

        public List<DataModel.Task> GetAll()
        {
            if (!File.Exists(JsonFile))
                File.Create(JsonFile).Close();
            JsonData = File.ReadAllText(JsonFile);
            return JsonConvert.DeserializeObject<List<DataModel.Task>>(JsonData) ?? new List<DataModel.Task>();
        }

        public void WriteData(List<DataModel.Task> tasksList, string jsonData)
        {
            File.WriteAllText(JsonFile, JsonConvert.SerializeObject(tasksList, Formatting.Indented));
        }
    }
}