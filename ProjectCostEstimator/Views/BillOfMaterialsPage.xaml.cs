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

    private async void AddNewItemButton_Clicked(object sender, EventArgs e)
    {
        var page = new EditItemPage();

        // Subscribe to the Disappearing event of the EditItemPage
        // When the EditItemPage disappears, check if the item has a title and add it to the Items collection
        page.Disappearing += (s, args) =>
        {
            if (page.Item != null)
            {
                Items.Add(page.Item);
                // Refresh the ListView
                BOMCollectionView.ItemsSource = null;
                BOMCollectionView.ItemsSource = Items;
            }
        };

        await Navigation.PushAsync(page);
    }
}