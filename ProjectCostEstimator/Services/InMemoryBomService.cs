using System.Collections.ObjectModel;
using ProjectCostEstimator.Models;

namespace ProjectCostEstimator.Services;

public class InMemoryBomService : IBomService
{
    public ObservableCollection<MaterialItem> Items { get; } = new()
    {
        new MaterialItem("Test1", "url1", "ea", 25.2m, 300m),
        new MaterialItem("Test2", "url2", "ea", 45.2m, 1000m)
    };

    public void AddItem(MaterialItem item)
    {
        Items.Add(item);
    }

    public void ReplaceItem(MaterialItem original, MaterialItem replacement)
    {
        int index = Items.IndexOf(original);
        if (index < 0)
            return;

        Items[index] = replacement;
    }

    public void RemoveItem(MaterialItem item)
    {
        Items.Remove(item);
    }
}
