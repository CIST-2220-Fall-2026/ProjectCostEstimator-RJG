using ProjectCostEstimator.Models;
using ProjectCostEstimator.ViewModels;

namespace ProjectCostEstimator.Views;

public partial class BillOfMaterialsPage : ContentPage
{
    private readonly BillOfMaterialsViewModel viewModel;

    public BillOfMaterialsPage(BillOfMaterialsViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        viewModel.AddRequested += OnAddRequested;
        viewModel.EditRequested += OnEditRequested;
        viewModel.DeleteRequested += OnDeleteRequested;
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

    private async void OnEditRequested(object? sender, MaterialItemEventArgs e)
    {
        MaterialItem originalItem = e.Item;
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

    private async void OnDeleteRequested(object? sender, MaterialItemEventArgs e)
    {
        MaterialItem item = e.Item;
        bool result = await DisplayAlertAsync("Confirm Deletion", $"Are you sure you want to delete {item.Title}?", "Yes", "No");
        
        if (!result)
        {
            return;
        }

        viewModel.RemoveItem(item);
    }
}
