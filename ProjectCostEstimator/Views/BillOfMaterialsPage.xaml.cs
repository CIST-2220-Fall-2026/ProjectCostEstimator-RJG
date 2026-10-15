using ProjectCostEstimator.Models;
using ProjectCostEstimator.ViewModels;

namespace ProjectCostEstimator.Views;

public partial class BillOfMaterialsPage : ContentPage
{
    private readonly BillOfMaterialsViewModel viewModel;

    public BillOfMaterialsPage()
    {
        InitializeComponent();
        viewModel = new BillOfMaterialsViewModel();
        viewModel.AddRequested += OnAddRequested;
        viewModel.EditRequested += OnEditRequested;
        viewModel.DeleteRequested += OnDeleteRequested;
        viewModel.ItemsChanged += OnItemsChanged;
        BindingContext = viewModel;
    }

    private async void OnAddRequested(object? sender, EventArgs e)
    {
        var page = new ItemEditorPage();

        // Add the item when the editor closes after a successful Submit.
        page.Disappearing += (s, args) =>
        {
            if (page.Item != null)
            {
                viewModel.AddItem(page.Item);
            }
        };

        await Navigation.PushAsync(page);
    }

    private async void OnEditRequested(object? sender, MaterialItem originalItem)
    {
        var page = new ItemEditorPage(originalItem);

        page.Disappearing += (s, args) =>
        {
            // Submit creates a new item. Cancel leaves Item pointing at originalItem.
            if (page.Item is not MaterialItem editedItem ||
                ReferenceEquals(editedItem, originalItem))
            {
                return;
            }

            viewModel.ReplaceItem(originalItem, editedItem);
        };

        await Navigation.PushAsync(page);
    }

    private async void OnDeleteRequested(object? sender, MaterialItem item)
    {
        bool result = await DisplayAlertAsync("Confirm Deletion", $"Are you sure you want to delete {item.Title}?", "Yes", "No");
        
        if (!result)
        {
            return;
        }

        viewModel.RemoveItem(item);
    }

    private void OnItemsChanged(object? sender, EventArgs e)
    {
        // Items is still a List, so CollectionView needs a manual refresh.
        BOMCollectionView.ItemsSource = null;
        BOMCollectionView.ItemsSource = viewModel.Items;
    }
}
