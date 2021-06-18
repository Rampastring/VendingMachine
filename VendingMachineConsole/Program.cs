using System;
using System.Collections.Generic;
using VendingMachineLibrary;
using VendingMachineLibrary.Items;
using VendingMachineLibrary.Logging;

namespace VendingMachineConsole
{
    /// <summary>
    /// A simple console application for testing the vending machine.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var vendingMachine = new VendingMachine();
            vendingMachine.AddLogger(new ConsoleLogger());
            vendingMachine.AddItem(new WeaponItem("Knife", 50));
            vendingMachine.AddItem(new WeaponItem("AK-74", 200));
            vendingMachine.AddItem(new WeaponItem("AK-47", 100));
            var reindeerMeatItem = new FoodItem("Reindeer Meat", 250);
            vendingMachine.AddItems(reindeerMeatItem, 5);

            PrintCatalogue(vendingMachine.GetCatalogue());

            vendingMachine.ReduceQuantity(reindeerMeatItem, 1);
            PrintCatalogue(vendingMachine.GetCatalogue());

            vendingMachine.ReduceQuantity(new FoodItem("Reindeer Meat", 250), 1);
            PrintCatalogue(vendingMachine.GetCatalogue());

            Console.WriteLine("Attempting to buy reindeer meat for 200, result: " + vendingMachine.Buy(reindeerMeatItem, 200));
            PrintCatalogue(vendingMachine.GetCatalogue());

            Console.WriteLine("Attempting to buy reindeer meat for 300, result: " + vendingMachine.Buy(reindeerMeatItem, 300));
            PrintCatalogue(vendingMachine.GetCatalogue());

            vendingMachine.RemoveItem(reindeerMeatItem);
            PrintCatalogue(vendingMachine.GetCatalogue());

            vendingMachine.ClearLoggers();
            vendingMachine.AddLogger(new AmazingCustomLogger());
            vendingMachine.RemoveItem(new WeaponItem("AK-47", 100));
        }

        static void PrintCatalogue(List<VendingMachineEntry> catalogue)
        {
            Console.WriteLine();
            Console.WriteLine("Catalogue:");
            catalogue.ForEach(entry => Console.WriteLine(entry));
            Console.WriteLine();
        }
    }
}
