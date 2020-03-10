using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Data.Model
{
    public class Task
    {
        [PrimaryKey]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public Nullable<DateTime> CompletedDate { get; set; }
        public int Priority { get; set; }
    }
}