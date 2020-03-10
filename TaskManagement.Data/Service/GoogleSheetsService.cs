using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Data.IServices;
using TaskManagement.Model.Model;

namespace TaskManagement.Data.Service
{
    public class GoogleSheetsService : IJobService
    {
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };

        static readonly string ApplicationName = "Time Manager";

        static readonly string SpreadSheetId = "136mw3ERxAMkh1O0UWxZhIIXiLXYOOqTXmMpQ2gQYO2w";

        static readonly string Sheet = "MainSheet";

        static SheetsService SheetsService;

        public GoogleSheetsService()
        {
            GoogleCredential googleCredential;
            using (var stream = new FileStream("C:\\Users\\udayt\\Desktop\\Edikate.PayrollManager-master\\Edikate.PayrollManager-master\\Edikate.PayrollManager.App\\client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                googleCredential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }

            SheetsService = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = googleCredential,
                ApplicationName = ApplicationName,
            });
            //ReadEntries();
            //CreateEntry();
        }

        public void Add(Job task)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Job Get(string id)
        {
            throw new NotImplementedException();
        }

        public List<Job> GetAll()
        {
            var range = $"{Sheet}!A:F";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    SheetsService.Spreadsheets.Values.Get(SpreadSheetId, range);

            var response = request.Execute();
            IList<IList<object>> values = response.Values;
            if (values != null && values.Count > 0)
            {

                return values.Cast<Job>().ToList();
            }
            else
            {
                return new List<Job>();
            }
        }

        public void Update(Job job)
        {
            throw new NotImplementedException();
        }
    }
}
