using ProjectCostEstimator.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ProjectCostEstimator.ViewModels
{
    public class EditItemViewModel : INotifyPropertyChanged
    {
        public EditItemViewModel(MaterialItem? item)
        {
            Item = item;
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        public string? Title { get; set; }
        public string? SourceUrl { get; set; }
        public string? Units { get; set; }
        public string? CostPerUnit { get; set; }
        public string? UnitsNeeded { get; set; }
        public string TotalText { get; private set; } = "Please enter item details and click Calculate.";
        public string? ErrorText { get; set; }

        public MaterialItem? Item { get; set; }

        public void Submit()
        {
            // If null instantiate a new one
            if (Item == null)
            {
                Item = new MaterialItem();
            }
            // Instantiate an object and set its properties
            // Get the values from the input fields
            Item.Title = Title ?? string.Empty;
            Item.SourceUrl = SourceUrl?? string.Empty;
            Item.Units = Units ?? string.Empty;
            Item.CostPerUnit = decimal.Parse(CostPerUnit ?? string.Empty);
            Item.UnitsNeeded = decimal.Parse(UnitsNeeded ?? string.Empty);
        }
    }
}
