using ProjectCostEstimator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCostEstimator.ViewModels
{
    public class BillOfMaterialsViewModel
    {
        public List<MaterialItem> Items { get; } = new()
        {
            new MaterialItem("Test1", "url1", "ea", 25.2m, 300m),
            new MaterialItem("Test2", "url2", "ea", 45.2m, 1000m)
        };


    }
}
