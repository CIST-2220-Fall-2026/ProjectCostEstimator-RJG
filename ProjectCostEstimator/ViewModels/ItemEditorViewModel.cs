using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjectCostEstimator.Models;

namespace ProjectCostEstimator.ViewModels
{
    public partial class ItemEditorViewModel:ObservableObject
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        public partial string Title { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string SourceURL { get; set; } = string.Empty;

        [ObservableProperty]

        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        public partial string Units { get; set; } = string.Empty;

        [ObservableProperty]

        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        public partial string CostPerUnitText { get; set; } = string.Empty;

        [ObservableProperty]

        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        public partial string UnitsNeededText { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string TotalText { get; set; } = "Enter item details and tap Submit.";

        [ObservableProperty]
        public partial string ValidationText { get; set; } = "Enter an item title and units.";
        public event EventHandler<ItemSubmittedEventArgs>? Submitted;
        public event EventHandler? CancelRequested;

        public ItemEditorViewModel(MaterialItem? item = null)
        {          
            if (item == null)
            {
                return;
            }
            Title = item.Title;
            SourceURL = item.SourceUrl;
            Units = item.Units;
            CostPerUnitText = item.CostPerUnit.ToString();
            UnitsNeededText = item.UnitsNeeded.ToString();
        }
      
        private bool CanSubmit()
        {
            return TryValidate(out _, out _, out _);
        }

        private bool TryValidate(out decimal cost, out decimal quantity, out string message)
        {
            cost = 0;
            quantity = 0;

            if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Units))
            {
                message = "Enter an item title and units.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(CostPerUnitText) || string.IsNullOrWhiteSpace(UnitsNeededText))
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
                SourceURL.Trim(), 
                Units.Trim(), 
                cost,  
                quantity);

            Submitted?.Invoke(this, new ItemSubmittedEventArgs(item));
        }

        [RelayCommand]
        private void Cancel()
        {
            CancelRequested?.Invoke(this, EventArgs.Empty);
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
}
