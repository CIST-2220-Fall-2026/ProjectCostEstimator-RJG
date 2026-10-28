using ProjectCostEstimator.Models;
using ProjectCostEstimator.ViewModels;

namespace ProjectCostEstimator.Views;

public partial class ItemEditorPage : ContentPage
{
    private readonly ItemEditorViewModel viewModel;
    public MaterialItem? Item { get; private set; } = null;

    public ItemEditorPage(MaterialItem? item = null)
    {
        InitializeComponent();
        Item = item;
        viewModel = new ItemEditorViewModel(item);
        viewModel.Submitted += OnSubmitted;
        viewModel.CancelRequested += OnCancelRequested;
        BindingContext = viewModel;        
    }

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
