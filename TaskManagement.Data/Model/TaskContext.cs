using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Data.Model
{
    public class TaskContext : DbContext
    {
        public TaskContext(string connection) : base(connection)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<TaskContext>());
        }

        public DbSet<Task> Tasks { get; set; }
    }
}