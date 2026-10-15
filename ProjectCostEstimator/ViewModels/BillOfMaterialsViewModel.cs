using System.Windows.Input;
using ProjectCostEstimator.Models;

namespace ProjectCostEstimator.ViewModels;

public class BillOfMaterialsViewModel
{
    public List<MaterialItem> Items { get; } = new()
    {
        new MaterialItem("Test1", "url1", "ea", 25.2m, 300m),
        new MaterialItem("Test2", "url2", "ea", 45.2m, 1000m)
    };

    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }

    public event EventHandler? AddRequested;
    public event EventHandler<MaterialItem>? EditRequested;
    public event EventHandler<MaterialItem>? DeleteRequested;
    public event EventHandler? ItemsChanged;

    public BillOfMaterialsViewModel()
    {
        AddCommand = new Command(() => AddRequested?.Invoke(this, EventArgs.Empty));
        EditCommand = new Command<MaterialItem>(item =>
        {
            if (item is not null && Items.Contains(item))
                EditRequested?.Invoke(this, item);
        });
        DeleteCommand = new Command<MaterialItem>(item =>
        {
            if (item is not null && Items.Contains(item))
                DeleteRequested?.Invoke(this, item);
        });
    }

    public void AddItem(MaterialItem item)
    {
        Items.Add(item);
        ItemsChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ReplaceItem(MaterialItem original, MaterialItem replacement)
    {
        int index = Items.IndexOf(original);
        if (index < 0)
            return;

        Items[index] = replacement;
        ItemsChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItem(MaterialItem item)
    {
        if (Items.Remove(item))
            ItemsChanged?.Invoke(this, EventArgs.Empty);
    }
}
