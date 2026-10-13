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
        MaterialItem? item = null;
        await Navigation.PushAsync(new EditItemPage(item));
        if (item != null) 
        {
            Items.Add(item);
            // Refresh the listview
            BOMCollectionView.ItemsSource = null;
            BOMCollectionView.ItemsSource = Items;
        }
    }
}