using VendingMachine.Items;

namespace VendingMachine
{
    /// <summary>
    /// An item type entry in the vending machine.
    /// Contains the item and information on its stock.
    /// </summary>
    public class VendingMachineEntry
    {
        public VendingMachineEntry(Item item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }

        public Item Item { get; }
        public int Quantity { get; internal set; }
    }
}
