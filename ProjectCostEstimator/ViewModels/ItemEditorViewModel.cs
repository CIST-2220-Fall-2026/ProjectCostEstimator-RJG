using ProjectCostEstimator.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ProjectCostEstimator.ViewModels
{
    public class ItemEditorViewModel:INotifyPropertyChanged
    {
        public ItemEditorViewModel():this(null)
        {                
        }

        public ItemEditorViewModel(MaterialItem? item)
        {
            this.item = item;
            if(item != null)
            {
                title = item.Title;
                sourceURL = item.SourceUrl;
                units = item.Units;
                costPerUnitText = item.CostPerUnit.ToString();
                unitsNeededText = item.UnitsNeeded.ToString();
                totalText = item.TotalCost.ToString();
            }
        }

        MaterialItem? item;

        public event PropertyChangedEventHandler? PropertyChanged;

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
            set { SetProperty(ref units, value, nameof(Units)); }
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
    }
}
