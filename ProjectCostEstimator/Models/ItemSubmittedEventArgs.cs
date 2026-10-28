using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCostEstimator.Models
{
    public sealed class ItemSubmittedEventArgs : EventArgs
    {
        public MaterialItem Item { get; private set; }

        public ItemSubmittedEventArgs(MaterialItem item)
        {
            Item = item;
        }
    }
}
