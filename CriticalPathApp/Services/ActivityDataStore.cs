using CriticalPathApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CriticalPathApp.Services
{
    public class ActivityDataStore : IDataStore<ActivityModel>
    {
        private List<ActivityModel> Activities;
        public async Task<bool> AddItemAsync(ActivityModel item)
        {
            Activities.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> ClearAllItemAsync()
        {
            Activities.Clear();
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = Activities.Where((ActivityModel arg) => arg.Id == id).FirstOrDefault();
            Activities.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<ActivityModel> GetItemAsync(string id)
        {
            return await Task.FromResult(Activities.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<ActivityModel>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(Activities);
        }

        public async Task<bool> UpdateItemAsync(ActivityModel item)
        {
            var oldItem = Activities.Where((ActivityModel arg) => arg.Id == item.Id).FirstOrDefault();
            Activities.Remove(oldItem);
            Activities.Add(item);

            return await Task.FromResult(true);
        }
    }
}
