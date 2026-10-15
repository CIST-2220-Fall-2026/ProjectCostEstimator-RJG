namespace ProjectCostEstimator.Models;

public sealed class ItemSubmittedEventArgs : EventArgs
{
    public MaterialItem Item { get; }

    public ItemSubmittedEventArgs(MaterialItem item)
    {
        Item = item;
    }
}
