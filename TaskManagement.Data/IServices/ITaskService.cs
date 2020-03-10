using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel = TaskManagement.Data.Model;

namespace TaskManagement.Data.IServices
{
    public interface ITaskService
    {
        void Add(DataModel.Task task);
        void Update(DataModel.Task task);
        void Delete(Guid id);
        List<DataModel.Task> GetAll();
        DataModel.Task Get(Guid id);
    }
}