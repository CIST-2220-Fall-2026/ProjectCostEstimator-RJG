using Microsoft.Extensions.DependencyInjection;
using ProjectCostEstimator.Models;
using ProjectCostEstimator.Views;

namespace ProjectCostEstimator.Services;

public sealed class MauiBomInteractionService(IServiceProvider services) : IBomInteractionService
{
    public async Task<MaterialItem?> EditItemAsync(MaterialItem? original)
    {
        var page = services.GetRequiredService<ItemEditorPage>();
        page.LoadItem(original);

        var completion = new TaskCompletionSource<MaterialItem?>(
            TaskCreationOptions.RunContinuationsAsynchronously);
        void OnDisappearing(object? sender, EventArgs args)
        {
            page.Disappearing -= OnDisappearing;
            completion.TrySetResult(page.Item);
        }

        page.Disappearing += OnDisappearing;
        try
        {
            await Shell.Current.Navigation.PushAsync(page);
            return await completion.Task;
        }
        catch
        {
            page.Disappearing -= OnDisappearing;
            throw;
        }
    }

    public Task<bool> ConfirmDeleteAsync(MaterialItem item) =>
        Shell.Current.DisplayAlertAsync(
            "Confirm Deletion",
            $"Are you sure you want to delete {item.Title}?",
            "Yes", "No");
}
