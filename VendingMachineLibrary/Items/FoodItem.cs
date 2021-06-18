namespace VendingMachineLibrary.Items
{
    /// <summary>
    /// A food item for the vending machine.
    /// </summary>
    public class FoodItem : Item
    {
        public FoodItem(string name, int price) : base(name, price)
        {
        }

        public override ItemCategory Category => ItemCategory.Food;

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
