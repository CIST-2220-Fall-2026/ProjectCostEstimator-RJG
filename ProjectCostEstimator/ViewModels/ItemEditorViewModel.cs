using System.ComponentModel;
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

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Title
    {
        get { return title; }
        set { SetProperty(ref title, value, nameof(Title)); }
    }

    public string SourceUrl
    {
        get { return sourceUrl; }
        set { SetProperty(ref sourceUrl, value, nameof(SourceUrl)); }
    }

    public string Units
    {
        get { return units; }
        set { SetProperty(ref units, value, nameof(Units)); }
    }

    public string CostPerUnitText
    {
        get { return costPerUnitText; }
        set
        {
            if (SetProperty(ref costPerUnitText, value, nameof(CostPerUnitText)))
                UpdateTotalText();
        }
    }

    public string UnitsNeededText
    {
        get { return unitsNeededText; }
        set
        {
            if (SetProperty(ref unitsNeededText, value, nameof(UnitsNeededText)))
                UpdateTotalText();
        }
    }

    public string TotalText
    {
        get { return totalText; }
        private set { SetProperty(ref totalText, value, nameof(TotalText)); }
    }

    public ItemEditorViewModel(MaterialItem? item = null)
    {
        if (item is null)
            return;

        Title = item.Title;
        SourceUrl = item.SourceUrl;
        Units = item.Units;
        CostPerUnitText = item.CostPerUnit.ToString();
        UnitsNeededText = item.UnitsNeeded.ToString();
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
