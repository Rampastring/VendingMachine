using System;
using System.Collections.Generic;
using System.Linq;
using VendingMachineLibrary.Items;
using VendingMachineLibrary.Logging;

namespace VendingMachineLibrary
{
    /*
    • Vending machine has a catalogue of items
    • Items can have different prices
    • Vending machines can have different catalogue
    • Vending machine can be refilled with new items
    • Vending machine has an interface for buying items with money
    • Some items are food
    • Some items are drinks
    • Some items are weapons
    • Vending machine has debug logging
    • As a developer I can inject different type of logger implementation to the vending machine class*/

    // Tests with NUnit

    /// <summary>
    /// The main vending machine class.
    /// </summary>
    public class VendingMachine
    {
        private readonly List<ILogger> _loggers = new List<ILogger>(0);

        private List<VendingMachineEntry> _catalogue = new List<VendingMachineEntry>();

        private readonly object _locker = new object();

        /// <summary>
        /// Adds an item to the vending machine with a quantity of one.
        /// If the vending machine already has the item listed, then adds one to the quantity of the item.
        /// </summary>
        /// <param name="item">The item to add to the vending machine.</param>
        public void AddItem(Item item)
        {
            AddItems(item, 1);
        }

        /// <summary>
        /// Adds a given number of specific items to the vending machine.
        /// </summary>
        /// <param name="item">The type of the items to add.</param>
        /// <param name="quantity">The quantity of items to add. Must be non-negative.
        /// If you want to remove items instead, use <see cref="ReduceQuantity(Item, int)"/>.</param>
        /// <exception cref="ArgumentException">If the given quantity is negative.</exception>
        /// <remarks>Does not change the quantity of the item if the quantity calculation would overflow.</remarks>
        public void AddItems(Item item, int quantity)
        {
            if (quantity < 0)
            {
                throw new ArgumentException("Quantity must be non-negative.");
            }

            lock (_locker)
            {
                var existing = FindItem(item);
                if (existing == null)
                {
                    _catalogue.Add(new VendingMachineEntry(item, quantity));
                    _catalogue = _catalogue.OrderBy(entry => entry.Item.Category).ThenBy(entry => entry.Item.Price).ToList();
                    Log($"Added new item to vending machine: '{item}', quantity {quantity}", LogLevel.Info);
                }
                else
                {
                    int newQuantity = existing.Quantity;
                    try
                    {
                        newQuantity += checked(existing.Quantity + quantity);
                    }
                    catch (OverflowException)
                    {
                        Log($"Integer overflow when restocking items in vending machine. " +
                            $"Item '{item}', current quantity {existing.Quantity}, attempted add {quantity}", LogLevel.Critical);
                        return;
                    }

                    existing.Quantity = newQuantity;

                    Log($"Changed stock of existing item in vending machine: '{item}', " +
                        $"added quantity {quantity}, total new quantity {existing.Quantity}", LogLevel.Info);
                }
            }
        }

        /// <summary>
        /// Removes all items of a specific type from the vending machine.
        /// </summary>
        /// <param name="item">The type of the items to remove.</param>
        /// <returns>True if removing the items was successful, false otherwise.</returns>
        public bool RemoveItem(Item item)
        {
            lock (_locker)
            {
                int index = _catalogue.FindIndex(e => e.Item.Equals(item));
                if (index > -1)
                {
                    _catalogue.RemoveAt(index);
                    Log($"Removed item from vending machine: '{item}'", LogLevel.Info);
                    return true;
                }

                Log($"Cannot find item to remove from vending machine: '{item}'", LogLevel.Warning);
                return false;
            }
        }

        /// <summary>
        /// Reduces the quantity of items of a specific type from the vending machine.
        /// </summary>
        /// <param name="item">The type of the item to reduce the quantity of.</param>
        /// <param name="quantity">The number of items that will be removed. Must be non-negative.</param>
        /// <returns>True if reducing the quantity of the item was successful, false otherwise.</returns>
        /// <exception cref="ArgumentException">If the given quantity is negative.</exception>
        /// <remarks>Sets the quantity for the item to zero if it would be reduced to negative. 
        /// Does not remove the item even if its quantity reached zero; for removing items, use <see cref="RemoveItem(Item)"/> instead.</remarks>
        public bool ReduceQuantity(Item item, int quantity)
        {
            if (quantity < 0)
            {
                throw new ArgumentException("Quantity must be non-negative.");
            }

            lock (_locker)
            {
                var entry = FindItem(item);
                if (entry == null)
                {
                    Log("Cannot find item to reduce quantity of from vending machine: " + item, LogLevel.Warning);
                    return false;
                }

                entry.Quantity -= quantity;
                if (entry.Quantity < 0)
                {
                    Log($"Quantity for '{item}' reduced to negative ({entry.Quantity}; " +
                        $"was reduced by {quantity}). Setting quantity to zero.", LogLevel.Warning);
                }
                else
                {
                    Log($"Quantity for '{item}' reduced by {quantity} to {entry.Quantity}. ", LogLevel.Info);
                }

                return true;
            }
        }

        /// <summary>
        /// Returns a copy of the vending machine's catalogue.
        /// Manipulating the returned list does not affect
        /// the vending machine. However, manipulating objects
        /// of the list will affect the items in the vending machine.
        /// </summary>
        public List<VendingMachineEntry> GetCatalogue()
        {
            lock (_locker)
            {
                return new List<VendingMachineEntry>(_catalogue);
            }
        }

        /// <summary>
        /// Completely clears the vending machine's catalogue.
        /// </summary>
        public void ClearCatalogue()
        {
            lock (_locker)
            {
                _catalogue.Clear();
            }
        }

        /// <summary>
        /// Buys an item from the vending machine.
        /// </summary>
        /// <param name="item">The item to buy.</param>
        /// <param name="money">The amount of money available for buying the item.</param>
        /// <returns>True if the item was bought successfully, false otherwise.</returns>
        /// <remarks>If buying the item fails, log entries will more information.</remarks>
        public bool Buy(Item item, int money)
        {
            lock (_locker)
            {
                var entry = FindItem(item);
                if (entry == null)
                {
                    Log($"Item '{item}' does not exist in the vending machine!", LogLevel.Warning);
                    return false;
                }

                var entryItem = entry.Item;

                if (money < entryItem.Price)
                {
                    Log($"Cannot buy item '{item}', not enough money given!", LogLevel.Warning);
                    return false;
                }

                if (entry.Quantity <= 0)
                {
                    Log($"Cannot buy item '{item}', it has no stock in the vending machine!", LogLevel.Warning);
                    return false;
                }

                entry.Quantity--;
                Log($"Item bought: '{item}'", LogLevel.Info);
                return true;
            }
        }

        /// <summary>
        /// Adds a logger to the vending machine.
        /// </summary>
        /// <param name="logger">The logger to add.</param>
        public void AddLogger(ILogger logger)
        {
            _loggers.Add(logger);
        }

        /// <summary>
        /// Removes a logger from the vending machine.
        /// </summary>
        /// <param name="logger">The logger to remove.</param>
        public void RemoveLogger(ILogger logger)
        {
            _loggers.Remove(logger);
        }

        /// <summary>
        /// Clears all loggers from the vending machine.
        /// </summary>
        public void ClearLoggers()
        {
            _loggers.Clear();
        }

        private void Log(string line, LogLevel logLevel)
        {
            foreach (var logger in _loggers)
            {
                logger.Log(line, logLevel);
            }
        }

        private VendingMachineEntry FindItem(Item item)
        {
            return _catalogue.Find(entry => entry.Item.Equals(item));
        }
    }
}
