using ProjectCostEstimator.Models;

namespace ProjectCostEstimator.Views;

public partial class BillOfMaterialsPage : ContentPage
{
    public List<MaterialItem> Items { get; set; } = new()
    {
        new MaterialItem("Test1","url1","ea",25.2m,300m),
        new MaterialItem("Test2","url2","ea",45.2m,1000m)
    };

	public BillOfMaterialsPage()
	{
		InitializeComponent();
        BindingContext = this;
	}

    private async void AddNewItemButton_Clicked(object? sender, EventArgs e)
    {
        var page = new ItemEditorPage();

        // Add the item when the editor closes after a successful Submit.
        page.Disappearing += (s, args) =>
        {
            if (page.Item != null)
            {
                Items.Add(page.Item);
                // Refresh the CollectionView because Items is a List.
                BOMCollectionView.ItemsSource = null;
                BOMCollectionView.ItemsSource = Items;
            }
        };

        await Navigation.PushAsync(page);
    }

    private async void EditButton_Clicked(object? sender, EventArgs e)
    {
        if (sender is not Button button ||
        button.BindingContext is not MaterialItem originalItem)
        {
            return;
        }

        var page = new ItemEditorPage(originalItem);

        page.Disappearing += (s, args) =>
        {
            // Submit creates a new item. Cancel leaves Item pointing at originalItem.
            if (page.Item is not MaterialItem editedItem ||
                ReferenceEquals(editedItem, originalItem))
            {
                return;
            }

            int index = Items.IndexOf(originalItem);
            if (index < 0)
            {
                return;
            }

            Items[index] = editedItem; // Replace the row; do not add another one.

            // Items is currently a List, so notify the CollectionView manually.
            BOMCollectionView.ItemsSource = null;
            BOMCollectionView.ItemsSource = Items;
        };

        await Navigation.PushAsync(page);
    }

    private async void DeleteButton_Clicked(object? sender, EventArgs e)
    {
        if (sender is not Button button ||
            button.BindingContext is not MaterialItem item)
        {
            return;
        }

        //Confirm deletion with the user
        bool result = await DisplayAlertAsync("Confirm Deletion", $"Are you sure you want to delete {item.Title}?", "Yes", "No");
        
        if (!result)
        {
            return;
        }

        Items.Remove(item);

        // Notify the CollectionView manually.
        BOMCollectionView.ItemsSource = null;
        BOMCollectionView.ItemsSource = Items;
    }
}