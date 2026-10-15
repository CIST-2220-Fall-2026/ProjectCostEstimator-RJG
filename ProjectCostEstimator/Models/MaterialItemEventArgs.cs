namespace ProjectCostEstimator.Models;

public sealed class MaterialItemEventArgs : EventArgs
{
    public MaterialItem Item { get; }

    public MaterialItemEventArgs(MaterialItem item)
    {
        Item = item;
    }
}
