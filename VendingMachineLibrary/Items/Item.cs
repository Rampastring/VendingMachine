namespace VendingMachineLibrary.Items
{
    /// <summary>
    /// Abstract base class for all vending machine items.
    /// </summary>
    public abstract class Item
    {
        public Item(string name, int price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; set; }
        public int Price { get; set; }
        public abstract ItemCategory Category { get; }

        public override string ToString()
        {
            return $"{Name}, price: {Price}, category: {Category}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (!(obj is Item objAsItem))
                return false;

            return objAsItem.Name == Name && objAsItem.Price == Price && objAsItem.Category == Category;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() / 10 + (Price * 10) + (int)Category;
        }
    }
}
