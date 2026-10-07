using ProjectCostEstimator.Models;

namespace ProjectCostEstimator.Views;

public partial class ItemCalculatorPage : ContentPage
{
	public ItemCalculatorPage()
	{
		InitializeComponent();
	}

    private void CalculateButton_Clicked(object sender, EventArgs e)
    {
        // Get the values from the input fields
        string title = TitleEntry.Text ?? string.Empty;
		string sourceUrl = SourceUrlEntry.Text ?? string.Empty;
		string units = UnitsEntry.Text ?? string.Empty;
		decimal costPerUnit = decimal.Parse(CostPerUnitEntry.Text);
		decimal unitsNeeded = decimal.Parse(UnitsNeededEntry.Text);

        // Instantiate an object and set its properties
        MaterialItem item = new(title, sourceUrl, units, costPerUnit, unitsNeeded);

        // Display the appropriate thing from the ojbect
        TotalCostLabel.Text = item.ToString();
    }
}