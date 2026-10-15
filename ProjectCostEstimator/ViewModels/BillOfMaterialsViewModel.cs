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

    [ObservableProperty]
    public partial string ErrorText { get; set; }

    [RelayCommand]
    private async Task LoadAsync()
    {
        try
        {
            await bomService.LoadAsync();
            ErrorText = string.Empty;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or System.Text.Json.JsonException)
        {
            ErrorText = $"Could not load the bill of materials: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task AddAsync()
    {
        MaterialItem? item = await interactions.EditItemAsync(null);
        if (item is not null)
        {
            try { await bomService.AddItemAsync(item); ErrorText = string.Empty; }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            { ErrorText = $"Could not save the item: {ex.Message}"; }
        }
    }

    [RelayCommand]
    private async Task EditAsync(MaterialItem? original)
    {
        if (original is null || !Items.Contains(original))
            return;

        MaterialItem? edited = await interactions.EditItemAsync(original);
        if (edited is not null)
        {
            try { await bomService.ReplaceItemAsync(original, edited); ErrorText = string.Empty; }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            { ErrorText = $"Could not save the edit: {ex.Message}"; }
        }
    }

    [RelayCommand]
    private async Task DeleteAsync(MaterialItem? item)
    {
        if (item is null || !Items.Contains(item))
            return;

        if (await interactions.ConfirmDeleteAsync(item))
        {
            try { await bomService.RemoveItemAsync(item); ErrorText = string.Empty; }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            { ErrorText = $"Could not save the deletion: {ex.Message}"; }
        }
    }
}
