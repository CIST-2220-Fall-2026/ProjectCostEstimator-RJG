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
        viewModel.Submitted += OnSubmitted;
        BindingContext = viewModel;        
    }

    private async void OnSubmitted(object? sender, ItemSubmittedEventArgs e)
    {
        Item = e.Item;
        await Navigation.PopAsync();
    }

    private async void CancelButton_Clicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
