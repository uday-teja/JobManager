using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Model.Model;

namespace TaskManagement.Data.IServices
{
    public interface IJobService
    {
        void Add(Job task);

        void Update(Job task);

        void Delete(string id);

        List<Job> GetAll();

        Job Get(string id);
    }
}
