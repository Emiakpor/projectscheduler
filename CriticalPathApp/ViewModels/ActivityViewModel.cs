using CriticalPathApp.Models;
using CriticalPathApp.Services;
using CriticalPathApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace CriticalPathApp.ViewModels
{
    public class ActivityViewModel : BaseViewModel
    {
        public ActivityViewModel()
        {
            RefreshCommand = new Command(CmdRefresh);
            CriticalPathCommand = new Command(FindCriticalPath);
            LoadItemsCommand = new Command(LoadActiviies);
            AddCommand = new Command(AddActivity); 
            RemoveCommand = new Command(RemoveActivity);
            EarliestStartCommand = new Command(EarliestStartCommandHandler);
            SampleDataCommand = new Command(SampleDataCommandHandler); 
            ClearCommand = new Command(ClearActivities); 
            SetActivities();
            Mode = "Add";
        }

        private void SampleDataCommandHandler(object obj)
        {
            SetActivities();
        }

        private List<string> GetActivityIds()
        {
            var activityIds = Activities.Select(a => a.Activity).ToList();
            return activityIds;
        }
        private void EarliestStartCommandHandler()
        {
           var activityIds = GetActivityIds();
            activityIds.Sort();
            foreach (string id in activityIds)
            {
               
            }
            for(int i=0; i<activityIds.Count; i++)
            {
                var _activity = Activities.FirstOrDefault(a => a.Id == activityIds[i]);

                if (i == 0)
                {
                    _activity.EarliestStart = 0;
                    _activity.EarliestFinish = _activity.EarliestStart + _activity.Duration;
                }
                else
                {
                    var predecessors = _activity.Predecessor.Split(',');
                    int np = predecessors.Length;
                    var earliestStart = 0;
                    foreach(var item in predecessors)
                    {
                        var _activityIn = Activities.FirstOrDefault(a => a.Id == item);
                        if (earliestStart < _activityIn.EarliestFinish) earliestStart = int.Parse(_activityIn.EarliestFinish.ToString());

                    }
                    _activity.EarliestStart = earliestStart;
                    _activity.EarliestFinish = _activity.EarliestStart + _activity.Duration;

                }
            }
        }

        private void AddActivity()
        {
            if (Mode == "Add")
            {
               AddNewActivity();
            }
            else if (Mode == "Edit")
            {
                EditActivity();
            }
        }

        private void AddNewActivity()
        {
            var newActivity = new ActivityModel
            {
                Sno = Activities.Count + 1,
                Id = Activity,
                Activity = Activity,
                Predecessor = Predecessor,
                Duration = Duration
            };

            Activities.Add(newActivity);
            ClearActivity();
        }

        private void EditActivity()
        {
            var _activity = Activities.FirstOrDefault(a => a.Id == SelectedActivity.Id);
            _activity.Activity = Activity;
            _activity.Predecessor = Predecessor;
            _activity.Duration = Duration;
            ClearActivity();

        }

        private void ClearActivity()
        {
            Activity = string.Empty;
            Predecessor = string.Empty;
            Duration = null;
        }

        private void SetActivity()
        {
            Activity = SelectedActivity.Activity;
            Predecessor = SelectedActivity.Predecessor;
            Duration = SelectedActivity.Duration;
            Mode = "Edit";
        }
        private void RemoveActivity()
        {
            Activities.RemoveAt(Activities.Count- 1);
        }

        private async void CmdRefresh(object obj)
        {
            IsRefreshing= true;
            await Task.Delay(2000);
            IsRefreshing= false;
        }
        private void SetActivities()
        {
            Activities = new ObservableCollection<ActivityModel>
            {
                new ActivityModel("A", 1, "A", "", 3, null, null),
                new ActivityModel("B", 2, "B", "A", 3, null, null),
                new ActivityModel("C", 3, "C", "A", 2, null, null),
                new ActivityModel("D", 4, "D", "B", 8,  null, null),
                new ActivityModel("E", 5, "E", "B", 8, null, null),
                new ActivityModel("F", 6, "F", "C,D", 6,  null, null),
                new ActivityModel("G", 7, "G", "E", 5,  null, null),
                new ActivityModel("H", 8, "H", "G", 24,  null, null),
                new ActivityModel("I", 9, "I", "H", 6, null, null),
                new ActivityModel("J", 10,"J","I,F", 3, null, null),
                new ActivityModel("K", 11,"K","J", 3, null, null),
                new ActivityModel("L", 12,"L","K", 2,  null, null),
                new ActivityModel("M", 13,"M", "L", 1,  null, null)
            };
        }
        private void SetActivitiesq()
        {
            Activities = new ObservableCollection<ActivityModel>
            {
                new ActivityModel("A", 1, "A", "", 3, 0, 3),
                new ActivityModel("B", 2, "B", "A", 3, 3, 6),
                new ActivityModel("C", 3, "C", "A", 2, 3, 5),
                new ActivityModel("D", 4, "D", "B", 8, 6, 14),
                new ActivityModel("E", 5, "E", "B", 8, 6, 14),
                new ActivityModel("F", 6, "F", "C,D", 6, 14, 20),
                new ActivityModel("G", 7, "G", "E", 5, 14, 19),
                new ActivityModel("H", 8, "H", "G", 24, 19, 43),
                new ActivityModel("I", 9, "I", "H", 6, 43, 49),
                new ActivityModel("J", 10,"J","I,F", 3, 49, 52),
                new ActivityModel("K", 11,"K","J", 3, 52, 55),
                new ActivityModel("L", 12,"L","K", 2, 55, 57),
                new ActivityModel("M", 13,"M", "L", 1, 57, 58)
            };
            //foreach (ActivityModel activityModel in _activities)
            //{
            //    await DataStore.AddItemAsync(activityModel);
            //}
        }

        private void ClearActivities()
        {
            Activities.Clear();
            Activities = new ObservableCollection<ActivityModel>();
        }

        private async void LoadActiviies()
        {
            var _activities = await DataStore.GetItemsAsync(true);
            foreach (var item in _activities)
            {
                Activities.Add(item);
            }
        }

        private void FindCriticalPath()
        {
            var _activities = CriticalPathCalculationService.GetActivities(Activities.ToList());
            CriticalPathCalculationService criticalPathCalculationService = new CriticalPathCalculationService(_activities);
            CriticalPath = criticalPathCalculationService.GetCriticalPaths();
            TotalDuration = criticalPathCalculationService.GetTotalDuration();
        }

        public ObservableCollection<ActivityModel> Activities { get; set; }

        private ActivityModel selectedActivity;

        public ActivityModel SelectedActivity
        {
            get { return selectedActivity; }
            set { selectedActivity = value; OnPropertyChanged(); SetActivity(); }
        }

        private bool isRefreshing;

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { isRefreshing = value; OnPropertyChanged(); }
        }

        private string criticalPath;

        public string CriticalPath
        {
            get { return criticalPath; }
            set { criticalPath = value; OnPropertyChanged(); }
        }

        private long totalDuration;

        public long TotalDuration
        {
            get { return totalDuration; }
            set { totalDuration = value; OnPropertyChanged(); }
        }

        private string activity;

        public string Activity
        {
            get { return activity; }
            set { activity = value; OnPropertyChanged(); }
        }

        private string predecessor;

        public string Predecessor
        {
            get { return predecessor; }
            set { predecessor = value; OnPropertyChanged(); }
        }

        private int? duration;

        public int? Duration
        {
            get { return duration; }
            set { duration = value; OnPropertyChanged(); }
        }

        private string mode;

        public string Mode
        {
            get { return mode; }
            set { mode = value; OnPropertyChanged(); }
        }

        public ICommand RefreshCommand { get; set; }
        public ICommand CriticalPathCommand { get; set; }
        public ICommand AddCommand { get; set; }
        
        public ICommand RemoveCommand { get; set; }
        public ActivityModel ActivityModel { get; set; }
        public Command LoadItemsCommand { get; }
        public Command ClearCommand { get; }
        public Command EarliestStartCommand { get; }
        public Command SampleDataCommand { get; }

        

    }
}
