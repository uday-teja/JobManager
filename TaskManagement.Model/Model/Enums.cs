using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Model.Model
{
    public enum Status
    {
        New = 0,
        InProgress = 1,
        Completed = 2,
    }

    public enum Priority
    {
        Low = 0,
        Medium = 1,
        High = 2,
    }

    public enum TaskState
    {
        New = 0,
        Update = 1,
    }

    //public enum DatabaseType
    //{
    //    Json = 0,
    //    SqlLite = 1,
    //    SqlServer = 2,
    //}

    //public enum AuthenticationType
    //{
    //    Windows = 0,
    //    SQLServer = 1,
    //}
}