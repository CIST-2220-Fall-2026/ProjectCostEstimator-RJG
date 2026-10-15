using System.Collections.ObjectModel;
using System.Windows.Input;
using ProjectCostEstimator.Models;

namespace ProjectCostEstimator.ViewModels;

public class BillOfMaterialsViewModel
{
    public ObservableCollection<MaterialItem> Items { get; } = new()
    {
        new MaterialItem("Test1", "url1", "ea", 25.2m, 300m),
        new MaterialItem("Test2", "url2", "ea", 45.2m, 1000m)
    };

    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }

    public event EventHandler? AddRequested;
    public event EventHandler<MaterialItemEventArgs>? EditRequested;
    public event EventHandler<MaterialItemEventArgs>? DeleteRequested;

    public BillOfMaterialsViewModel()
    {
        AddCommand = new Command(() => AddRequested?.Invoke(this, EventArgs.Empty));
        EditCommand = new Command<MaterialItem>(item =>
        {
            if (item is not null && Items.Contains(item))
                EditRequested?.Invoke(this, new MaterialItemEventArgs(item));
        });
        DeleteCommand = new Command<MaterialItem>(item =>
        {
            if (item is not null && Items.Contains(item))
                DeleteRequested?.Invoke(this, new MaterialItemEventArgs(item));
        });
    }

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
