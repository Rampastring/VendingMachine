using System;
using VendingMachineLibrary.Items;

namespace VendingMachineLibrary
{
    /// <summary>
    /// An item type entry in the vending machine.
    /// Contains the item and information on its stock.
    /// </summary>
    public class VendingMachineEntry
    {
        public VendingMachineEntry(Item item, int quantity)
        {
            Item = item ?? throw new ArgumentNullException(nameof(item));
            Quantity = quantity < 0 ? throw new ArgumentException("Quantity must be non-negative") : quantity;
        }

        public Item Item { get; }
        public int Quantity { get; internal set; }

        public override string ToString()
        {
            return Item.ToString() + ", quantity " + Quantity;
        }
    }
}
