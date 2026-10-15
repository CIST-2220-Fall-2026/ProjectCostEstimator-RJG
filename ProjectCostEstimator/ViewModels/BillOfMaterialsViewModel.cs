using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjectCostEstimator.Models;
using ProjectCostEstimator.Services;

namespace ProjectCostEstimator.ViewModels;

public partial class BillOfMaterialsViewModel : ObservableObject
{
    private readonly IBomService bomService;

    public ObservableCollection<MaterialItem> Items => bomService.Items;

    public event EventHandler? AddRequested;
    public event EventHandler<MaterialItemEventArgs>? EditRequested;
    public event EventHandler<MaterialItemEventArgs>? DeleteRequested;

    public BillOfMaterialsViewModel(IBomService bomService)
    {
        this.bomService = bomService;
    }

    [RelayCommand]
    private void Add() => AddRequested?.Invoke(this, EventArgs.Empty);

    [RelayCommand]
    private void Edit(MaterialItem? item)
    {
        if (item is not null && Items.Contains(item))
            EditRequested?.Invoke(this, new MaterialItemEventArgs(item));
    }

    [RelayCommand]
    private void Delete(MaterialItem? item)
    {
        if (item is not null && Items.Contains(item))
            DeleteRequested?.Invoke(this, new MaterialItemEventArgs(item));
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
