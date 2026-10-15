using ProjectCostEstimator.Models;
using ProjectCostEstimator.ViewModels;

namespace ProjectCostEstimator.Views;

public partial class ItemEditorPage : ContentPage
{
    private readonly ItemEditorViewModel viewModel;

    public MaterialItem? Item { get; private set; } = null;

    public ItemEditorPage(ItemEditorViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        viewModel.Submitted += OnSubmitted;
        viewModel.CancelRequested += OnCancelRequested;
        BindingContext = viewModel;
    }

    public void LoadItem(MaterialItem? item) => viewModel.LoadItem(item);

    private async void OnSubmitted(object? sender, ItemSubmittedEventArgs e)
    {
        Item = e.Item;
        await Navigation.PopAsync();
    }

    private async void OnCancelRequested(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
