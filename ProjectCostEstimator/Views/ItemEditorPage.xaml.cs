using ProjectCostEstimator.Models;

namespace ProjectCostEstimator.Views;

public partial class ItemEditorPage : ContentPage
{
    public MaterialItem? Item { get; private set; } = null;

    public ItemEditorPage():this(null)
	{		
	}

    public ItemEditorPage(MaterialItem? item)
    {
        InitializeComponent();
        Item = item;
        if (item is null)
            return;

        TitleEntry.Text = item.Title;
        SourceUrlEntry.Text = item.SourceUrl;
        UnitsEntry.Text = item.Units;
        CostPerUnitEntry.Text = item.CostPerUnit.ToString();
        UnitsNeededEntry.Text = item.UnitsNeeded.ToString();
    }

    private async void SubmitButton_Clicked(object? sender, EventArgs e)
    {
        TotalCostLabel.Text = "Enter item details and tap Submit.";

        if (string.IsNullOrWhiteSpace(TitleEntry.Text) ||
            string.IsNullOrWhiteSpace(UnitsEntry.Text))
        {
            await DisplayAlertAsync("Missing details",
                "Enter an item title and units.", "OK");
            return;
        }

        if (!decimal.TryParse(CostPerUnitEntry.Text, out decimal costPerUnit) ||
            !decimal.TryParse(UnitsNeededEntry.Text, out decimal unitsNeeded) ||
            costPerUnit < 0 || unitsNeeded < 0)
        {
            await DisplayAlertAsync("Invalid numbers",
                "Enter nonnegative numbers for cost and quantity.", "OK");
            return;
        }

        try
        {
            Item = new(
                TitleEntry.Text.Trim(),
                SourceUrlEntry.Text?.Trim() ?? string.Empty,
                UnitsEntry.Text.Trim(),
                costPerUnit,
                unitsNeeded);

            await Navigation.PopAsync();
        }
        catch (OverflowException)
        {
            await DisplayAlertAsync("Numbers too large",
                "Enter smaller values for cost and quantity.", "OK");
        }
    }

    private async void CancelButton_Clicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
