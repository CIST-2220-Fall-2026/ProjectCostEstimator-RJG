using ProjectCostEstimator.Models;
using System.Collections.ObjectModel;

namespace ProjectCostEstimator.Services
{
    public interface IBomService
    {
        ObservableCollection<MaterialItem> Items { get; }
        void AddItem(MaterialItem item);
        void ReplaceItem(MaterialItem original, MaterialItem replacement);
        void RemoveItem(MaterialItem item);
    }
}
