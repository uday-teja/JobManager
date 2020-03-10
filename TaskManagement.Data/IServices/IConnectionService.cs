using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Data.Model;

namespace TaskManagement.Data.IServices
{
    public interface IConnectionService
    {
        void DefaultSettings();
        Connection GetUserSettings();
        void SaveUserSettings(Connection connection);
        void DeleteUserSettings();
    }
}