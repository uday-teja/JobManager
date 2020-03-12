using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Model.Model
{
    public class Job : PropertyChangedBase
    {
        private string jobId;

        public string JobId
        {
            get { return jobId; }
            set
            {
                jobId = value;
                NotifyOfPropertyChange(() => JobId);
            }
        }

        private string machineType;

        public string MachineType
        {
            get { return machineType; }
            set
            {
                machineType = value;
                NotifyOfPropertyChange(() => MachineType);
            }
        }

        private string month;

        public string Month
        {
            get { return month; }
            set
            {
                month = value;
                NotifyOfPropertyChange(() => Month);
            }
        }

        private string companyName;

        public string CompanyName
        {
            get { return companyName; }
            set
            {
                companyName = value;
                NotifyOfPropertyChange(() => CompanyName);
            }
        }

        private string jobDetails;

        public string JobDetails
        {
            get { return jobDetails; }
            set
            {
                jobDetails = value;
                NotifyOfPropertyChange(() => JobDetails);
            }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                NotifyOfPropertyChange(() => Description);
            }
        }


        private string inDate;

        public string InDate
        {
            get { return inDate; }
            set
            {
                inDate = value;
                NotifyOfPropertyChange(() => InDate);
            }
        }

        private string expectedCompletionDate;

        public string ExpectedCompletionDate
        {
            get { return expectedCompletionDate; }
            set
            {
                expectedCompletionDate = value;
                NotifyOfPropertyChange(() => ExpectedCompletionDate);
            }
        }

        private string actualCompletionDate;

        public string ActualCompletionDate
        {
            get { return actualCompletionDate; }
            set
            {
                actualCompletionDate = value;
                NotifyOfPropertyChange(() => ActualCompletionDate);
            }
        }

        private string startTime;

        public string StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                NotifyOfPropertyChange(() => StartTime);
            }
        }

        private string endTime;

        public string EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value;
                NotifyOfPropertyChange(() => EndTime);
            }
        }

        private string totalTime;

        public string TotalTime
        {
            get { return totalTime; }
            set
            {
                totalTime = value;
                NotifyOfPropertyChange(() => TotalTime);
            }
        }

        private string status;

        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                NotifyOfPropertyChange(() => Status);
            }
        }

        private Stopwatch stopwatch;

        public Stopwatch Stopwatch
        {
            get { return stopwatch; }
            set
            {
                stopwatch = value;
                NotifyOfPropertyChange(() => Stopwatch);
            }
        }
    }
}
