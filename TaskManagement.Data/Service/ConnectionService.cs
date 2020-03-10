using System;
using System.IO;
using System.Linq;
using System.Text;
using TaskManagement.Data.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using TaskManagement.Data.IServices;

namespace TaskManagement.Data.Service
{
    public class ConnectionService : IConnectionService
    {
        public void DefaultSettings()
        {
        }

        public Connection GetUserSettings()
        {
            if (!File.Exists(Constants.UserSettings))
                File.Create(Constants.UserSettings).Close();
            return JsonConvert.DeserializeObject<Connection>(File.ReadAllText(Constants.UserSettings)) ?? new Connection();
        }

        public void SaveUserSettings(Connection connection)
        {
            File.WriteAllText(Constants.UserSettings, JsonConvert.SerializeObject(connection, Formatting.Indented));
        }

        public void DeleteUserSettings()
        {
            File.Delete(Constants.UserSettings);
        }
    }
}