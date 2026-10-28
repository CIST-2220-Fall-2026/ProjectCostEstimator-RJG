using System.ComponentModel;
using System.Windows.Input;
using ProjectCostEstimator.Models;

namespace ProjectCostEstimator.ViewModels
{
    public class ItemEditorViewModel:INotifyPropertyChanged
    {
        public ItemEditorViewModel():this(null)
        {                
        }

        public ItemEditorViewModel(MaterialItem? item)
        {
            submitCommand = new Command(Submit, CanSubmit);
            this.item = item;
            if (item == null)
            {
                return;
            }
            title = item.Title;
            sourceURL = item.SourceUrl;
            units = item.Units;
            costPerUnitText = item.CostPerUnit.ToString();
            unitsNeededText = item.UnitsNeeded.ToString();
            totalText = item.TotalCost.ToString();
        }

        private readonly Command submitCommand;
        public ICommand SubmitCommand { get { return submitCommand; } }

        public event EventHandler<ItemSubmittedEventArgs>? Submitted;
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

        private bool CanSubmit()
        {
            return TryValidate(out _, out _, out _);
        }

        private void Submit()
        {
            if (!TryValidate(out decimal cost, out decimal quantity, out string message))
            {
                ValidationText = message;
                return;
            }

            MaterialItem item = new(Title.Trim(), SourceURL.Trim(), Units.Trim(), cost,  quantity);
            Submitted?.Invoke(this, new ItemSubmittedEventArgs(item));
        }

        MaterialItem? item;

        public event PropertyChangedEventHandler? PropertyChanged;

        private string validationText = "Enter an item title and units.";
        public string ValidationText
        {
            get { return validationText; }
            private set { SetProperty(ref validationText, value, nameof(ValidationText)); }
        }

        private string title = string.Empty;
        public string Title 
        {
            get { return title; }
            set { SetProperty(ref title, value, nameof(Title)); }
        }

        private string sourceURL = string.Empty;
        public string SourceURL
        {
            get { return sourceURL; }
            set { SetProperty(ref sourceURL, value, nameof(SourceURL)); }
        }

        private string units = string.Empty;

        public string Units
        {
            get { return units; }
            set 
            {
                if (SetProperty(ref units, value, nameof(Units)))
                {
                    UpdateFormState();
                }
            }
        }

        private string costPerUnitText = string.Empty;

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

        private string unitsNeededText = string.Empty;

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

        public string totalText = "Enter item details and tap Submit.";
        public string TotalText
        {
            get { return totalText; }

            private set { SetProperty(ref totalText, value, nameof(TotalText)); }
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

        private bool SetProperty(ref string field, string? value, string propertyName)
        {
            value ??= string.Empty;
            if (field == value)
            {
                return false;
            }
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        private void UpdateFormState()
        {
            TryValidate(out _, out _, out string message);
            ValidationText = message;
            submitCommand.ChangeCanExecute();
        }
    }
}
