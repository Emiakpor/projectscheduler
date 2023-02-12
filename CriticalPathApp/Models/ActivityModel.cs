using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace CriticalPathApp.Models
{
    public class ActivityModel:BaseModel
    {
        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged(); }
        }
        private int sno;

        public int Sno
        {
            get { return sno; }
            set { sno = value; OnPropertyChanged(); }
        }

        private string activity;

        public string Activity
        {
            get { return activity; }
            set { activity = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Brief description concerning the activity.
        /// </summary>

        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged(); }
        }


        /// <summary>
        /// Total amount of time taken by the activity.
        /// </summary>
        private int? duration;

        public int? Duration
        {
            get { return duration; }
            set { duration = value; OnPropertyChanged(); }
        }


        /// <summary>
        /// Activities that come before the activity.
        /// </summary>
        private string predecessor;

        public string Predecessor
        {
            get { return predecessor; }
            set { predecessor = value; OnPropertyChanged(); }
        }

        /// </summary>
        private int? earliestStart;

        public int? EarliestStart
        {
            get { return earliestStart; }
            set { earliestStart = value; OnPropertyChanged(); }
        }

        private int? earliestFinish;

        public int? EarliestFinish
        {
            get { return earliestFinish; }
            set { earliestFinish = value; OnPropertyChanged(); }
        }

        private int? latestStart;

        public int? LatestStart
        {
            get { return latestStart; }
            set { latestStart = value; OnPropertyChanged(); }
        }

        private int? latestFinish;

        public int? LatestFinish
        {
            get { return latestFinish; }
            set { latestFinish = value; OnPropertyChanged(); }
        }

        private int? startSlack;

        public int? StartSlack
        {
            get { return startSlack; }
            set { startSlack = value; OnPropertyChanged(); }
        }

        private int? finishSlack;

        public int? FinishSlack
        {
            get { return finishSlack; }
            set { finishSlack = value; OnPropertyChanged(); }
        }
        public ICollection<ActivityModel> Predecessors { get; private set; } = new List<ActivityModel>();
        public ActivityModel()
        {

        }
        public ActivityModel(string id, int sno, string activity, string predecessor, int? duration = null, int? earliestStart=null, int? earliestFinish = null, int? latestStart = null, int? latestFinish = null, int? startSlack = null  , int? finishSlack = null, string description= null)
        {
            Id = id;
            Sno = sno;
            Activity = activity;
            Description = description;
            Duration = duration;
            Predecessor = predecessor;
            EarliestStart = earliestStart;
            EarliestFinish = earliestFinish;
            LatestStart = latestStart;
            LatestFinish = latestFinish;
            StartSlack = startSlack;
            FinishSlack = finishSlack;  
        }

       
    }
}
