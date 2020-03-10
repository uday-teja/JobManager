using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Model.Model;

namespace TaskManagement.Data.Model
{
    public class Connection
    {
        public string FileName { get; set; }
        public DatabaseType DatabaseType { get; set; }
        public AuthenticationType AuthenticationType { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}