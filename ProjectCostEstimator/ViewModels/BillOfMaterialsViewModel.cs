using System.Collections.ObjectModel;
using System.Windows.Input;
using ProjectCostEstimator.Models;
using ProjectCostEstimator.Services;

namespace ProjectCostEstimator.ViewModels;

public class BillOfMaterialsViewModel
{
    private readonly IBomService bomService;

    public ObservableCollection<MaterialItem> Items => bomService.Items;

    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }

    public event EventHandler? AddRequested;
    public event EventHandler<MaterialItemEventArgs>? EditRequested;
    public event EventHandler<MaterialItemEventArgs>? DeleteRequested;

    public BillOfMaterialsViewModel(IBomService bomService)
    {
        this.bomService = bomService;
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
        bomService.AddItem(item);
    }

    public void ReplaceItem(MaterialItem original, MaterialItem replacement)
    {
        bomService.ReplaceItem(original, replacement);
    }

    public void RemoveItem(MaterialItem item)
    {
        bomService.RemoveItem(item);
    }
}
