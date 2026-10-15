using ProjectCostEstimator.ViewModels;

namespace ProjectCostEstimator.Views;

public partial class BillOfMaterialsPage : ContentPage
{
    private readonly BillOfMaterialsViewModel viewModel;

    public BillOfMaterialsPage(BillOfMaterialsViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.LoadCommand.ExecuteAsync(null);
    }
}
