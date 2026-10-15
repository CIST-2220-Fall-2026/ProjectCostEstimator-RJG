using System.Collections.ObjectModel;
using ProjectCostEstimator.Models;

namespace ProjectCostEstimator.Services;

public interface IBomService
{
    ObservableCollection<MaterialItem> Items { get; }

    Task LoadAsync();

    Task AddItemAsync(MaterialItem item);

    Task ReplaceItemAsync(MaterialItem original, MaterialItem replacement);

    Task RemoveItemAsync(MaterialItem item);
}
