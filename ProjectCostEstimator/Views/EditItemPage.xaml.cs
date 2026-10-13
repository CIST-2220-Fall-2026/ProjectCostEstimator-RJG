using ProjectCostEstimator.Models;

namespace ProjectCostEstimator.Views;

public partial class EditItemPage : ContentPage
{
    public MaterialItem? Item { get; set; }
	public EditItemPage():this(null)
	{		
	}

    public EditItemPage(MaterialItem? item)
    {
        Item = item;
        InitializeComponent();
    }



    private void SubmitButton_Clicked(object sender, EventArgs e)
    {     
        // If null instantiate a new one
        if(Item == null)
        {
            Item = new MaterialItem();
        }
        // Instantiate an object and set its properties
        // Get the values from the input fields
        Item.Title = TitleEntry.Text ?? string.Empty;
        Item.SourceUrl = SourceUrlEntry.Text ?? string.Empty;
        Item.Units = UnitsEntry.Text ?? string.Empty;
        Item.CostPerUnit = decimal.Parse(CostPerUnitEntry.Text);
        Item.UnitsNeeded = decimal.Parse(UnitsNeededEntry.Text);

        // Go back previous view
        Navigation.PopAsync();
    }
}