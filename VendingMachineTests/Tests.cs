using NUnit.Framework;
using System;
using VendingMachineLibrary;
using VendingMachineLibrary.Items;

namespace VendingMachineTests
{
    /// <summary>
    /// Tests for the vending machine.
    /// </summary>
    public class Tests
    {
        private VendingMachine _vendingMachine;
        private readonly FoodItem reindeerMeatItem = new FoodItem("Reindeer Meat", 250);
        private readonly FoodItem gyozaItem = new FoodItem("Gyoza", 100);
        private readonly WeaponItem knifeWeaponItem = new WeaponItem("Knife", 100);

        [SetUp]
        public void Setup()
        {
            _vendingMachine = new VendingMachine();
        }

        [Test]
        public void AddItem_ItemNotInCatalogue_AddsItemToCatalogue()
        {
            Assert.IsEmpty(_vendingMachine.GetCatalogue());

            _vendingMachine.AddItem(reindeerMeatItem);
            Assert.IsTrue(_vendingMachine.GetCatalogue().Exists(entry => entry.Quantity == 1 && entry.Item.Equals(reindeerMeatItem)));
            Assert.AreEqual(_vendingMachine.GetCatalogue().Count, 1);
        }

        [Test]
        public void AddItem_ItemAlreadyInCatalogue_IncreasesQuantity()
        {
            _vendingMachine.AddItem(reindeerMeatItem);
            Assert.AreEqual(_vendingMachine.GetCatalogue().Count, 1);

            // Test addition by creating an equivalent object
            _vendingMachine.AddItem(new FoodItem(reindeerMeatItem.Name, reindeerMeatItem.Price));
            Assert.AreEqual(_vendingMachine.GetCatalogue().Count, 1);
            Assert.IsTrue(_vendingMachine.GetCatalogue().Exists(entry => entry.Quantity == 2 && entry.Item.Equals(reindeerMeatItem)));
        }

        [Test]
        public void AddItems_ItemNotInCatalogue_AddsItemToCatalogue()
        {
            const int quantity = 6;
            _vendingMachine.AddItems(reindeerMeatItem, quantity);
            Assert.AreEqual(_vendingMachine.GetCatalogue().Count, 1);
            Assert.IsTrue(_vendingMachine.GetCatalogue().Exists(entry => entry.Quantity == quantity && entry.Item.Equals(reindeerMeatItem)));
        }

        [Test]
        public void AddItems_ItemAlreadyInCatalogue_AddsQuantity()
        {
            const int quantity = 6;
            const int totalQuantity = quantity * 2;
            _vendingMachine.AddItems(reindeerMeatItem, quantity);
            Assert.AreEqual(_vendingMachine.GetCatalogue().Count, 1);

            // Test addition by creating an equivalent object
            _vendingMachine.AddItems(new FoodItem(reindeerMeatItem.Name, reindeerMeatItem.Price), quantity);
            Assert.AreEqual(_vendingMachine.GetCatalogue().Count, 1);
            Assert.IsTrue(_vendingMachine.GetCatalogue().Exists(entry => entry.Quantity == totalQuantity && entry.Item.Equals(reindeerMeatItem)));
        }

        [Test]
        public void AddItem_IntegerOverflow_ThrowsOverflowException()
        {
            _vendingMachine.AddItems(reindeerMeatItem, int.MaxValue);
            Assert.Throws(typeof(OverflowException), () => _vendingMachine.AddItem(reindeerMeatItem));
        }

        [Test]
        public void AddItems_IntegerOverflow_ThrowsOverflowException()
        {
            _vendingMachine.AddItems(reindeerMeatItem, int.MaxValue);
            Assert.Throws(typeof(OverflowException), () => _vendingMachine.AddItems(reindeerMeatItem, 1));
        }

        [Test]
        public void AddItems_NegativeQuantity_ThrowsArgumentException()
        {
            Assert.Throws(typeof(ArgumentException), () => _vendingMachine.AddItems(reindeerMeatItem, -1));
        }

        [Test]
        public void AddItem_NullItem_ThrowsArgumentNullException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => _vendingMachine.AddItem(null));
        }

        [Test]
        public void AddItems_NullItem_ThrowsArgumentNullException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => _vendingMachine.AddItems(null, 1));
        }

        [Test]
        public void RemoveItem_ItemInCatalogue_RemovesItem()
        {
            // Test removal by passing the same object reference
            _vendingMachine.AddItems(reindeerMeatItem, 10);
            _vendingMachine.AddItems(knifeWeaponItem, 10);
            Assert.AreEqual(_vendingMachine.GetCatalogue().Count, 2);
            _vendingMachine.RemoveItem(reindeerMeatItem);
            Assert.AreEqual(_vendingMachine.GetCatalogue().Count, 1);

            // Test removal by creating an equivalent object
            _vendingMachine.AddItems(reindeerMeatItem, 10);
            Assert.AreEqual(_vendingMachine.GetCatalogue().Count, 2);
            _vendingMachine.RemoveItem(new FoodItem(reindeerMeatItem.Name, reindeerMeatItem.Price));
            Assert.AreEqual(_vendingMachine.GetCatalogue().Count, 1);
            Assert.IsTrue(_vendingMachine.GetCatalogue().Exists(entry => entry.Item.Equals(knifeWeaponItem)));
        }

        [Test]
        public void RemoveItem_ItemNotInCatalogue_HasNoEffect()
        {
            _vendingMachine.AddItems(gyozaItem, 10);
            _vendingMachine.AddItems(knifeWeaponItem, 10);
            var oldCatalogue = _vendingMachine.GetCatalogue();
            _vendingMachine.RemoveItem(reindeerMeatItem);
            var newCatalogue = _vendingMachine.GetCatalogue();

            Assert.AreEqual(oldCatalogue.Count, newCatalogue.Count);
            for (int i = 0; i < oldCatalogue.Count; i++)
            {
                Assert.AreEqual(oldCatalogue[i], newCatalogue[i]);
            }
        }

        [Test]
        public void RemoveItem_NullItem_ThrowsArgumentNullException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => _vendingMachine.RemoveItem(null));
        }

        [Test]
        public void ReduceQuantity_ItemInCatalogue_ReducesQuantity()
        {
            const int oldQuantity = 10;
            const int reduction = 3;
            const int newQuantity = 4;
            
            _vendingMachine.AddItems(gyozaItem, oldQuantity);
            _vendingMachine.AddItems(reindeerMeatItem, oldQuantity);
            // Reduce amount of gyoza twice (once by object refence, once by equivalent object)
            _vendingMachine.ReduceQuantity(gyozaItem, reduction);
            _vendingMachine.ReduceQuantity(new FoodItem(gyozaItem.Name, gyozaItem.Price), reduction);

            // Check that the catalogue contains the expected amount of reindeer meat (not reduced) and gyoza
            var catalogue = _vendingMachine.GetCatalogue();
            Assert.AreEqual(catalogue.Count, 2);
            Assert.IsTrue(catalogue.Exists(entry => entry.Item == gyozaItem && entry.Quantity == newQuantity));
            Assert.IsTrue(catalogue.Exists(entry => entry.Item == reindeerMeatItem && entry.Quantity == oldQuantity));
        }

        [Test]
        public void ReduceQuantity_ItemNotInCatalogue_HasNoEffect()
        {
            _vendingMachine.AddItems(gyozaItem, 10);
            _vendingMachine.AddItems(knifeWeaponItem, 10);
            var oldCatalogue = _vendingMachine.GetCatalogue();
            _vendingMachine.ReduceQuantity(reindeerMeatItem, 5);
            var newCatalogue = _vendingMachine.GetCatalogue();

            Assert.AreEqual(oldCatalogue.Count, newCatalogue.Count);
            for (int i = 0; i < oldCatalogue.Count; i++)
            {
                Assert.AreEqual(oldCatalogue[i], newCatalogue[i]);
            }
        }

        [Test]
        public void ReduceQuantity_QuantityReducedToNegative_ChangesQuantityToZero()
        {
            const int quantity = 10;
            _vendingMachine.AddItems(gyozaItem, quantity);
            _vendingMachine.ReduceQuantity(gyozaItem, quantity + 1);
            var catalogue = _vendingMachine.GetCatalogue();
            Assert.AreEqual(catalogue.Count, 1);
            Assert.IsTrue(catalogue.Exists(entry => entry.Quantity == 0 && entry.Item.Equals(gyozaItem)));
        }

        [Test]
        public void ReduceQuantity_NegativeQuantity_ThrowsArgumentException()
        {
            Assert.Throws(typeof(ArgumentException), () => _vendingMachine.ReduceQuantity(reindeerMeatItem, -10));
        }

        [Test]
        public void ReduceQuantity_NullItem_ThrowsArgumentNullException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => _vendingMachine.ReduceQuantity(null, 10));
        }

        [Test]
        public void ClearCatalogue_ClearsCatalogue()
        {
            _vendingMachine.AddItems(gyozaItem, 10);
            _vendingMachine.AddItems(knifeWeaponItem, 10);
            _vendingMachine.ClearCatalogue();
            Assert.IsEmpty(_vendingMachine.GetCatalogue());
        }

        [Test]
        public void Buy_ItemHasStock_EnoughMoneyGiven_ItemIsBought()
        {
            int quantity = 10;

            _vendingMachine.AddItems(gyozaItem, quantity);

            // Test buying with object reference
            bool wasBought = _vendingMachine.Buy(gyozaItem, gyozaItem.Price);
            Assert.IsTrue(wasBought);

            // Test buying with equivalent object
            wasBought = _vendingMachine.Buy(new FoodItem(gyozaItem.Name, gyozaItem.Price), gyozaItem.Price + 1);
            Assert.IsTrue(wasBought);

            Assert.IsTrue(_vendingMachine.GetCatalogue().Exists(
                entry => entry.Quantity == quantity - 2 && entry.Item.Equals(gyozaItem)));
        }

        [Test]
        public void Buy_ItemHasStock_NotEnoughMoneyGiven_ItemIsNotBought()
        {
            int quantity = 10;

            _vendingMachine.AddItems(gyozaItem, quantity);

            bool wasBought = _vendingMachine.Buy(gyozaItem, gyozaItem.Price - 2);
            Assert.IsFalse(wasBought);
            Assert.IsTrue(_vendingMachine.GetCatalogue().Exists(
                entry => entry.Quantity == quantity && entry.Item.Equals(gyozaItem)));
        }

        [Test]
        public void Buy_ItemHasNoStock_EnoughMoneyGiven_ItemIsNotBought()
        {
            _vendingMachine.AddItems(gyozaItem, 0);

            bool wasBought = _vendingMachine.Buy(gyozaItem, gyozaItem.Price + 2);
            Assert.IsFalse(wasBought);
            Assert.IsTrue(_vendingMachine.GetCatalogue().Exists(
                entry => entry.Quantity == 0 && entry.Item.Equals(gyozaItem)));
        }

        [Test]
        public void Buy_NullItem_ThrowsArgumentNullException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => _vendingMachine.Buy(null, 1000));
        }
    }
}