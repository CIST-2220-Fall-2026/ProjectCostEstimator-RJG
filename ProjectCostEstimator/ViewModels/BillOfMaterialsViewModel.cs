using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjectCostEstimator.Models;
using ProjectCostEstimator.Services;

namespace ProjectCostEstimator.ViewModels;

public partial class BillOfMaterialsViewModel(
    IBomService bomService,
    IBomInteractionService interactions) : ObservableObject
{
    public ObservableCollection<MaterialItem> Items => bomService.Items;

    [RelayCommand]
    private async Task AddAsync()
    {
        MaterialItem? item = await interactions.EditItemAsync(null);
        if (item is not null)
            bomService.AddItem(item);
    }

    [RelayCommand]
    private async Task EditAsync(MaterialItem? original)
    {
        if (original is null || !Items.Contains(original))
            return;

        MaterialItem? edited = await interactions.EditItemAsync(original);
        if (edited is not null)
            bomService.ReplaceItem(original, edited);
    }

    [RelayCommand]
    private async Task DeleteAsync(MaterialItem? item)
    {
        if (item is null || !Items.Contains(item))
            return;

        if (await interactions.ConfirmDeleteAsync(item))
            bomService.RemoveItem(item);
    }
}
