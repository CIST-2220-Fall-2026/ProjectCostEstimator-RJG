namespace ProjectCostEstimator.Models
{
    public class MaterialItemEventArgs : EventArgs
    {
        public MaterialItem Item { get; }

        public MaterialItemEventArgs(MaterialItem item)
        {
            Item = item;
        }
    }
}
