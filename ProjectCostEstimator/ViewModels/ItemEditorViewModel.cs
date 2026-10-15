using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjectCostEstimator.Models;

namespace ProjectCostEstimator.ViewModels;

public partial class ItemEditorViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
    public partial string Title { get; set; }

    [ObservableProperty]
    public partial string SourceUrl { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
    public partial string Units { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
    public partial string CostPerUnitText { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
    public partial string UnitsNeededText { get; set; }

    [ObservableProperty]
    public partial string TotalText { get; set; }

    [ObservableProperty]
    public partial string ValidationText { get; set; }

    public event EventHandler<ItemSubmittedEventArgs>? Submitted;
    public event EventHandler? CancelRequested;

    public ItemEditorViewModel(MaterialItem? item = null)
    {
        Title = string.Empty;
        SourceUrl = string.Empty;
        Units = string.Empty;
        CostPerUnitText = string.Empty;
        UnitsNeededText = string.Empty;
        TotalText = "Enter cost and quantity to see a total.";
        ValidationText = "Enter an item title and units.";

        if (item is null)
            return;

        Title = item.Title;
        SourceUrl = item.SourceUrl;
        Units = item.Units;
        CostPerUnitText = item.CostPerUnit.ToString();
        UnitsNeededText = item.UnitsNeeded.ToString();
    }

    partial void OnTitleChanged(string value) => UpdateFormState();

    partial void OnUnitsChanged(string value) => UpdateFormState();

    partial void OnCostPerUnitTextChanged(string value)
    {
        UpdateTotalText();
        UpdateFormState();
    }

    partial void OnUnitsNeededTextChanged(string value)
    {
        UpdateTotalText();
        UpdateFormState();
    }

    private bool CanSubmit() => TryValidate(out _, out _, out _);

    private bool TryValidate(out decimal cost, out decimal quantity, out string message)
    {
        cost = 0;
        quantity = 0;

        if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Units))
        {
            message = "Enter an item title and units.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(CostPerUnitText) ||
            string.IsNullOrWhiteSpace(UnitsNeededText))
        {
            message = "Enter cost and quantity.";
            return false;
        }

        if (!decimal.TryParse(CostPerUnitText, out cost) ||
            !decimal.TryParse(UnitsNeededText, out quantity) ||
            cost < 0 || quantity < 0)
        {
            message = "Enter nonnegative numbers for cost and quantity.";
            return false;
        }

        try
        {
            _ = cost * quantity;
        }
        catch (OverflowException)
        {
            message = "Numbers are too large to calculate a total.";
            return false;
        }

        message = "Ready to submit.";
        return true;
    }

    private void UpdateFormState()
    {
        TryValidate(out _, out _, out string message);
        ValidationText = message;
    }

    [RelayCommand(CanExecute = nameof(CanSubmit))]
    private void Submit()
    {
        if (!TryValidate(out decimal cost, out decimal quantity, out string message))
        {
            ValidationText = message;
            return;
        }

        MaterialItem item = new(
            Title.Trim(),
            SourceUrl.Trim(),
            Units.Trim(),
            cost,
            quantity);

        Submitted?.Invoke(this, new ItemSubmittedEventArgs(item));
    }

    [RelayCommand]
    private void Cancel() => CancelRequested?.Invoke(this, EventArgs.Empty);

    private void UpdateTotalText()
    {
        if (string.IsNullOrWhiteSpace(CostPerUnitText) ||
            string.IsNullOrWhiteSpace(UnitsNeededText))
        {
            TotalText = "Enter cost and quantity to see a total.";
            return;
        }

        if (!decimal.TryParse(CostPerUnitText, out decimal cost) ||
            !decimal.TryParse(UnitsNeededText, out decimal quantity) ||
            cost < 0 || quantity < 0)
        {
            TotalText = "Enter nonnegative numbers for cost and quantity.";
            return;
        }

        try
        {
            TotalText = $"Estimated total: {cost * quantity:C}";
        }
        catch (OverflowException)
        {
            TotalText = "Numbers are too large to calculate a total.";
        }
    }
}
