using ProjectCostEstimator.Models;
using ProjectCostEstimator.ViewModels;

namespace ProjectCostEstimator.Views;

public partial class ItemEditorPage : ContentPage
{
    private readonly ItemEditorViewModel viewModel;
    public MaterialItem? Item { get; private set; } = null;

    public ItemEditorPage() : this(null)
	{		
	}

    public ItemEditorPage(MaterialItem? item)
    {
        InitializeComponent();
        Item = item;
        viewModel = new ItemEditorViewModel(item);
        BindingContext = viewModel;
    }

    private async void SubmitButton_Clicked(object? sender, EventArgs e)
    {
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
                viewModel.Title,
                viewModel.SourceURL,
                viewModel.Units,
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
