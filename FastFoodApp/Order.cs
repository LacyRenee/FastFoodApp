// Programmer: Lacy Tesdall
// Details: This stores all of the sandwiches, drinks, and fries for a user's order.

using System;
using System.Collections.Generic;
using System.Linq;
using static FastFoodApp.MainWindow;

namespace FastFoodApp
{
    class Order
    {
        private int drinks = 0;
        private int fries  = 0;
        private List<Sandwich> sandwiches = new List<Sandwich>();

        public Order() { }

        public Order(List<Sandwich> newSandwich, int newDrinks, int newFries)
        {
            this.sandwiches = newSandwich;
            this.drinks     = newDrinks;
            this.fries      = newFries;
        }

        public float calculateOrderPrice()
        {
            return calculateSandwichPrices() + calculateDrinkPrices() + calculateFriesPrices();
        }

        public float calculateTax()
        {
            return calculateOrderPrice() * TAX;
        }

        public float calculateSandwichPrices()
        {
            float sandwichPrice = 0.0F;

            foreach(Sandwich s in sandwiches)
            {
                sandwichPrice += s.getPrice();
            }

            return sandwichPrice;
        }

        public float calculateDrinkPrices() { return DRINK_PRICE * drinks; }

        public float calculateFriesPrices() { return FRIES_PRICE * fries; }

        public void addSandwich(Sandwich newSandwich)
        {
            if (newSandwich.Equals(null))
            {
                throw new ArgumentNullException("Cannot add a null sandwich to the list");
            }
            else
            {
                sandwiches.Add(newSandwich);
            }
        }

        public Sandwich getSandwich(int position) { return sandwiches.ElementAt(position); }

        public List<Sandwich> getSandwichList() { return sandwiches; }

        public void addDrinks(int newDrinks)
        {
            if (newDrinks == 1)
            {
                this.drinks++;
            }
            else if (newDrinks < 0)
            {
                throw new ArgumentOutOfRangeException("Drinks cannot be negative");
            }
        }

        public int getDrinks() { return this.drinks; }

        public void addFries(int newFries)
        {
            if (newFries == 1)
            {
                this.fries++;
            }
            else if (newFries < 0)
            {
                throw new ArgumentOutOfRangeException("Fries cannot be negative");
            }
        }

        public int getFries() { return this.fries; }
    }
}
