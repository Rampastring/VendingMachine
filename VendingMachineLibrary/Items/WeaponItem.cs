namespace VendingMachineLibrary.Items
{
    /// <summary>
    /// A weapon item for the vending machine.
    /// </summary>
    public class WeaponItem : Item
    {
        public WeaponItem(string name, int price) : base(name, price)
        {
        }

        public override ItemCategory Category => ItemCategory.Weapon;

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
