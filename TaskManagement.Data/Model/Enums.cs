using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Data.Model
{
    public enum DatabaseType
    {
        Json = 0,
        SqlLite = 1,
        SqlServer = 2,
    }

    public enum AuthenticationType
    {
        Windows = 0,
        SQLServer = 1,
    }
}
