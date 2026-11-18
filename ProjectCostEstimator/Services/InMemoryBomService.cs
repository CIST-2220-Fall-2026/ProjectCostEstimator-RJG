using ProjectCostEstimator.Models;
using System.Collections.ObjectModel;

namespace ProjectCostEstimator.Services
{
    public class InMemoryBomService : IBomService
    {
        public ObservableCollection<MaterialItem> Items { get; } = new();

        public Task LoadAsync()
        {
            return Task.CompletedTask;
        }

        public Task AddItemAsync(MaterialItem item)
        {
            Items.Add(item);
            return Task.CompletedTask;
        }

        public Task ReplaceItemAsync(MaterialItem original, MaterialItem replacement)
        {
            int index = Items.IndexOf(original);
            if (index >= 0)
            {
                Items[index] = replacement;
            }
            return Task.CompletedTask;
        }

        public Task RemoveItemAsync(MaterialItem item)
        {
            Items.Remove(item);
            return Task.CompletedTask;
        }
    }
}
