using ProjectCostEstimator.Models;
using ProjectCostEstimator.ViewModels;

namespace ProjectCostEstimator.Views;

public partial class EditItemPage : ContentPage
{
    private EditItemViewModel viewModel;

    public MaterialItem Item
    {
        get { return viewModel.Item; }
        set { viewModel.Item = value; }
    }

    public EditItemPage():this(null)
	{		
	}

    public EditItemPage(MaterialItem? item)
    {
        InitializeComponent();
        viewModel = new EditItemViewModel(item);
        BindingContext = viewModel;
    }

    private void SubmitButton_Clicked(object sender, EventArgs e)
    {     
        viewModel.Submit();

        // Go back previous view
        Navigation.PopAsync();
    }

    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        // Go back previous view
        Navigation.PopAsync();
    }
}