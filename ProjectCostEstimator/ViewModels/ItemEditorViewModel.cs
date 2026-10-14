using System.ComponentModel;
using System.Windows.Input;
using ProjectCostEstimator.Models;

namespace ProjectCostEstimator.ViewModels;

public class ItemEditorViewModel : INotifyPropertyChanged
{
    private string title = string.Empty;
    private string sourceUrl = string.Empty;
    private string units = string.Empty;
    private string costPerUnitText = string.Empty;
    private string unitsNeededText = string.Empty;
    private string totalText = "Enter cost and quantity to see a total.";
    private string validationText = "Enter an item title and units.";
    private readonly Command submitCommand;

    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<MaterialItem>? Submitted;
    public event EventHandler? CancelRequested;

    public ICommand SubmitCommand => submitCommand;
    public ICommand CancelCommand { get; }

    public string Title
    {
        get { return title; }
        set
        {
            if (SetProperty(ref title, value, nameof(Title)))
                UpdateFormState();
        }
    }

    public string SourceUrl
    {
        get { return sourceUrl; }
        set { SetProperty(ref sourceUrl, value, nameof(SourceUrl)); }
    }

    public string Units
    {
        get { return units; }
        set
        {
            if (SetProperty(ref units, value, nameof(Units)))
                UpdateFormState();
        }
    }

    public string CostPerUnitText
    {
        get { return costPerUnitText; }
        set
        {
            if (SetProperty(ref costPerUnitText, value, nameof(CostPerUnitText)))
            {
                UpdateTotalText();
                UpdateFormState();
            }
        }
    }

    public string UnitsNeededText
    {
        get { return unitsNeededText; }
        set
        {
            if (SetProperty(ref unitsNeededText, value, nameof(UnitsNeededText)))
            {
                UpdateTotalText();
                UpdateFormState();
            }
        }
    }

    public string TotalText
    {
        get { return totalText; }
        private set { SetProperty(ref totalText, value, nameof(TotalText)); }
    }

    public string ValidationText
    {
        get { return validationText; }
        private set { SetProperty(ref validationText, value, nameof(ValidationText)); }
    }

    public ItemEditorViewModel(MaterialItem? item = null)
    {
        submitCommand = new Command(Submit, CanSubmit);
        CancelCommand = new Command(() => CancelRequested?.Invoke(this, EventArgs.Empty));

        if (item is null)
            return;

        Title = item.Title;
        SourceUrl = item.SourceUrl;
        Units = item.Units;
        CostPerUnitText = item.CostPerUnit.ToString();
        UnitsNeededText = item.UnitsNeeded.ToString();
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
        submitCommand.ChangeCanExecute();
    }

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

        Submitted?.Invoke(this, item);
    }

    private bool SetProperty(ref string field, string? value, string propertyName)
    {
        value ??= string.Empty;

        if (field == value)
            return false;

        field = value;
        PropertyChanged?.Invoke(this,
            new PropertyChangedEventArgs(propertyName));
        return true;
    }

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
