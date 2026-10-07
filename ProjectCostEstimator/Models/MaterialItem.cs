using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCostEstimator.Models
{
    public class MaterialItem
    {
        public MaterialItem() : this(string.Empty, string.Empty, string.Empty, 0m, 0m)
        {
        }

        public MaterialItem(string title, string sourceUrl, string units, decimal costPerUnit, decimal unitsNeeded)
        {
            Title = title;
            SourceUrl = sourceUrl;
            Units = units;
            this.costPerUnit = costPerUnit;
            this.unitsNeeded = unitsNeeded;
            CalculateTotalCost();
        }

        public string Title { get; set; } = string.Empty;
        public string SourceUrl { get; set; } = string.Empty;
        public string Units { get; set; } = string.Empty;

        private decimal costPerUnit;        
        public decimal CostPerUnit
        {
            get { return costPerUnit; }
            set { costPerUnit = value; CalculateTotalCost(); }
        }

        private decimal unitsNeeded;
        public decimal UnitsNeeded
        {
            get { return unitsNeeded; }
            set { unitsNeeded = value; CalculateTotalCost(); }
        }

        public decimal TotalCost { get; private set; }

        private void CalculateTotalCost()
        {
            TotalCost = costPerUnit * unitsNeeded;
        }
        
        public override string ToString()
        {
            return $"{Title} - {UnitsNeeded} {Units} at {CostPerUnit:C} each. Total: {TotalCost:C}";
        }
    }
}
