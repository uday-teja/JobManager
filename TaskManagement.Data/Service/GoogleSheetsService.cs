using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using System;
using System.Collections;
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
            var range = $"{Sheet}!A:M";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    SheetsService.Spreadsheets.Values.Get(SpreadSheetId, range);

            var response = request.Execute();
            IList<IList<object>> values = response.Values;
            if (values != null && values.Count > 0)
            {

                return values.Select(CreateJob).ToList();
            }
            else
            {
                return new List<Job>();
            }
        }

        public Job CreateJob(object obj)
        {
            var jobList = (IList)obj;
            Job job = new Job();
            if (jobList.Count > 1)
                job.JobId = jobList[0]?.ToString();
            if (jobList.Count > 2)
                job.MachineType = jobList[1]?.ToString();
            if (jobList.Count > 3)
                job.Month = jobList[2]?.ToString();
            if (jobList.Count > 4)
                job.CompanyName = jobList[3]?.ToString();
            if (jobList.Count > 5)
                job.JobDetails = jobList[4]?.ToString();
            if (jobList.Count > 6)
                job.Description = jobList[5]?.ToString();
            if (jobList.Count > 7)
                job.InDate = jobList[6]?.ToString();
            if (jobList.Count > 8)
                job.ExpectedCompletionDate = jobList[7]?.ToString();
            if (jobList.Count > 9)
                job.ActualCompletionDate = jobList[8]?.ToString();
            if (jobList.Count > 10)
                job.Status = jobList[9]?.ToString();
            if (jobList.Count > 11)
                job.StartTime = jobList[10]?.ToString();
            if (jobList.Count > 12)
                job.EndTime = jobList[11]?.ToString();
            if (jobList.Count > 13)
                job.TotalTime = jobList[12]?.ToString();
            return job;
        }

        public void Update(Job job)
        {
            throw new NotImplementedException();
        }
    }
}
