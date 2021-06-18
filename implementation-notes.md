# Vending machine implementation notes

The description of the task didn't include a lot of details, so I had to make
some design choices and assumptions. I'll detail them in this document.

I figured that a typical vending machine has many items of the same type.
That's why I decided to have items of the same type grouped into a wrapper
with information on the quantity.

If the vending machine has A LOT of items, then a different data structure
than a list could be more efficient. But I'm assuming that the typical vending
machine has a few dozen different item types, for which the list is more than
efficient enough. With a more complex implementation it might've also been
beneficial for code maintainability to create a wrapper class for the
data structure.

Likewise, in a larger system it might've been beneficial
to have an ID for the different items for comparison, stored in a database etc.
Because of the simplicity of this system and a typical vending machine, I 
settled for just comparing properties when checking for quality.

The task description did not specify any requirements for the different item
types (food, drinks, weapons). Due to this, even just one Item class with a
Category field would have satisfied the requirements. However I assumed that
eventually, at a later stage, the developers would want the different item
types to have different properties (for example, a food item could have a
property for calories). So I made different classes for the item categories
for future extensibility.

I thought of making the Buy method more complex in a way that it allowed
buying multiple items of the same type at once. But, thinking about the domain,
I don't think I've seen a real-life vending machine that would allow buying more
than a single item at a time. So I made a simple buy method that can only be
used to buy a single item.

I made the vending machine keep its items sorted by category and price.

I assumed that there is no reason for the vending machine to ever have a negative
quantity of a specific item. I enforced this with checks. An alternative would've
been using unsigned integer types, but I felt that checks and documentation were clearer.

Unit test methods were roughly named after <Method-to-test>_<Cause>_<Effect>.

The vending machine should be safe to use with multiple threads at once,
with a simple lock implementation.

## Unit tests for logging

It would've been possible to implement unit tests for the logging part of the
vending machine library with the popular Moq framework (I'm familiar with it
from previous work). But doing it now would've taken just a bit too much time.
There's a very brief demonstration of logging injection in the 
VendingMachineConsole test console project.
