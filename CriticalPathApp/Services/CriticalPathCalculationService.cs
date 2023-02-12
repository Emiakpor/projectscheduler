using CriticalPathApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CriticalPathApp.Services
{
    public class CriticalPathCalculationService
    {
        public CriticalPathCalculationService(IEnumerable<ActivityModel> activities) {
            Output(activities.Shuffle().CriticalPath(p => p.Predecessors, l => (long)l.Duration));
        }
        public string GetCriticalPaths()
        {
            return _CriticalPaths;
        }

        public long GetTotalDuration()
        {
            return _TotalDuration;
        }

        public string CheckForLoops(IEnumerable<ActivityModel> activities)
        {
            var isCaughtProperly = false;
            var thread = new System.Threading.Thread(
                () =>
                {
                    try
                    {
                        activities.CriticalPath(p => p.Predecessors, l => (long)l.Duration);
                    }
                    catch (System.InvalidOperationException ex)
                    {
                        isCaughtProperly = true;
                    }
                }
                );
            thread.Start();
            for (var i = 0; i < 100; i++)
            {
                Thread.Sleep(100); // Wait for 10 seconds - our thread should finish by then
                if (thread.ThreadState != ThreadState.Running)
                    break;
            }
            if (thread.ThreadState == ThreadState.Running)
                thread.Abort();

            return isCaughtProperly ? "Critical path caught the loop" : "Critical path did not find the loop properly";
        }

        private void Output(IEnumerable<ActivityModel> list)
        {
            var sb = new StringBuilder();
            
            var totalDuration = 0L;
            foreach (ActivityModel activity in list)
            {
                Console.Write("{0} ", activity.Id);
                sb.AppendFormat("{0} ", activity.Id);
                totalDuration += int.Parse(activity.Duration.ToString());
            }
            sb.Remove(sb.Length - 1, 1);
            _CriticalPaths = sb.ToString();
            _TotalDuration = totalDuration;
        }

        public static IEnumerable<ActivityModel> GetActivities(List<ActivityModel> input)
        {
            var list = new List<ActivityModel>();
            var ad = new Dictionary<string, ActivityModel>();
            var deferredList = new Dictionary<ActivityModel, List<string>>();

            foreach (var line in input)
            {
                var activity = new ActivityModel();

                activity.Id = line.Id;
                activity.Activity = line.Activity;
                activity.Predecessor = line.Predecessor;
                activity.EarliestStart = line.EarliestStart;
                activity.EarliestFinish = line.EarliestFinish;
                ad.Add(activity.Activity, activity);
                activity.Description = line.Description;

                activity.Duration = line.Duration;

                var predecessors = line.Predecessor.Split(',');
                int np = predecessors.Length;

                if (np == 1 && predecessors[0] == String.Empty) np = 0;

                if (np != 0 )
                {
                    var allIds = new List<string>();
                    for (int j = 0; j < np; j++)
                    {
                        allIds.Add(predecessors[j].Trim());
                    }

                    if (allIds.Any(i => !ad.ContainsKey(i)))
                    {
                        // Defer processing on this one
                        deferredList.Add(activity, allIds);
                    }
                    else
                    {
                        foreach (var id in allIds)
                        {
                            var aux = ad[id];

                            activity.Predecessors.Add(aux);
                        }
                    }
                }
                list.Add(activity);
            }

            while (deferredList.Count > 0)
            {
                var processedActivities = new List<ActivityModel>();
                foreach (var activity in deferredList)
                {
                    if (activity.Value.Where(ad.ContainsKey).Count() == activity.Value.Count)
                    {
                        // All dependencies are now loaded
                        foreach (var id in activity.Value)
                        {
                            var aux = ad[id];

                            activity.Key.Predecessors.Add(aux);
                        }
                        processedActivities.Add(activity.Key);
                    }
                }
                foreach (var activity in processedActivities)
                {
                    deferredList.Remove(activity);
                }
            }

            return list;
        }

        private string _CriticalPaths = string.Empty;
        private long _TotalDuration = 0;
    }
}
