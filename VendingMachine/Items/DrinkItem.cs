namespace VendingMachine.Items
{
    /// <summary>
    /// A drink item for the vending machine.
    /// </summary>
    public class DrinkItem : Item
    {
        public DrinkItem(string name, int price) : base(name, price)
        {
        }

        public override ItemCategory Category => ItemCategory.Drink;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
