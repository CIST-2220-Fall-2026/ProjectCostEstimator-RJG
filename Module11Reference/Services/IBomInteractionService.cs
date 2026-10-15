using ProjectCostEstimator.Models;

namespace ProjectCostEstimator.Services;

// The view model asks for a result without depending on a MAUI page or dialog.
public interface IBomInteractionService
{
    Task<MaterialItem?> EditItemAsync(MaterialItem? original);

    Task<bool> ConfirmDeleteAsync(MaterialItem item);
}
