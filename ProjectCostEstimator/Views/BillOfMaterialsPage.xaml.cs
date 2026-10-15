using ProjectCostEstimator.ViewModels;

namespace ProjectCostEstimator.Views;

public partial class BillOfMaterialsPage : ContentPage
{
    public BillOfMaterialsPage(BillOfMaterialsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
