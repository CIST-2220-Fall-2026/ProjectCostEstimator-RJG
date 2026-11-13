using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjectCostEstimator.Models;
using ProjectCostEstimator.Services;
using System.Collections.ObjectModel;

namespace ProjectCostEstimator.ViewModels
{
    public partial class BillOfMaterialsViewModel : ObservableObject
    {
        private readonly IBomService bomService;
        private readonly IBomInteractionService interactions;
        public ObservableCollection<MaterialItem> Items { get { return bomService.Items; } }

        public BillOfMaterialsViewModel(IBomService bomService, IBomInteractionService interactions)
        {
            this.bomService = bomService;
            this.interactions = interactions;
        }

        [RelayCommand]
        private async Task AddAsync()
        {
            MaterialItem? item = await interactions.EditItemAsync(null);

            if (item is not null)
            {
                bomService.AddItem(item);
            }
        }

        [RelayCommand]
        private async Task EditAsync(MaterialItem original)
        {
            if (original is null || !Items.Contains(original))
            {
                return;
            }

            MaterialItem? edited = await interactions.EditItemAsync(original);

            if (edited is not null)
            {
                bomService.ReplaceItem(original, edited);
            }
        }
                
        [RelayCommand]
        private async Task DeleteAsync(MaterialItem item)
        {
            if (item is null || !Items.Contains(item))
            {
                return;
            }

            if (await interactions.ConfirmDeleteAsync(item))
            {
                bomService.RemoveItem(item);
            }
        }
    }
}
