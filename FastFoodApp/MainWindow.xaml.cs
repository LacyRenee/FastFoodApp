// Programmer: Lacy Tesdall
// Details: This page allows the user to create an order by adding a variety of meals, which includes the sandwich type, toppings, fries, and drinks.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FastFoodApp
{
    public enum SandwichTypes { Chicken, Turkey, Veggie, Fish }

    public enum Toppings { lettuce, tomato, pickles, onion, cheese, egg }

    public partial class MainWindow : Window
    {
        // Constants
        public const float CHICKEN_PRICE = 8.99F;
        public const float DRINK_PRICE   = 0.99F;
        public const float FISH_PRICE    = 5.99F;
        public const float FRIES_PRICE   = 1.99F;
        public const float TURKEY_PRICE  = 9.99F;
        public const float VEGGIE_PRICE  = 12.99F;
        public const float TAX           = 0.02F;

        // Structures
        public struct Sandwich
        {
            private SandwichTypes type;
            private float price;
            private List<String> toppings;

            public Sandwich(SandwichTypes newType, float newPrice, List<String> newToppings)
            {
                this.type     = newType;
                this.price    = newPrice;
                this.toppings = newToppings;
            }

            public SandwichTypes getType() { return this.type; }

            public void setType(SandwichTypes newType) { this.type = newType; }

            public float getPrice() { return price; }

            public void setPrice(float newPrice) { this.price = newPrice; }

            public String getToppings()
            {
                String toppingsList = String.Empty;

                foreach (String t in toppings)
                {
                    toppingsList += t + ' ';
                }

                return toppingsList;
            }
            public void setToppings(String newTopping) { this.toppings.Add(newTopping); }
        }

        // Variables
        private Sandwich sandwichOrder;
        private SandwichTypes sandwichType;
        private float sandwichPrice;
        private List<String> toppings;
        private Order finalOrder = new Order();
        private int numOfOrders  = 0;
        private int drinks       = 0;
        private int fries        = 0;

        public MainWindow()
        {

            InitializeComponent();

            // Initialize ComboBox with the sandwhich types and prices
            try
            {
                foreach (SandwichTypes s in Enum.GetValues(typeof(SandwichTypes)))
                {
                    switch (s)
                    {
                        case SandwichTypes.Chicken:
                            SandwichTypeList.Items.Add(s + String.Format(" - {0:C}", CHICKEN_PRICE));
                            break;
                        case SandwichTypes.Fish:
                            SandwichTypeList.Items.Add(s + String.Format(" - {0:C}", FISH_PRICE));
                            break;
                        case SandwichTypes.Turkey:
                            SandwichTypeList.Items.Add(s + String.Format(" - {0:C}", TURKEY_PRICE));
                            break;
                        case SandwichTypes.Veggie:
                            SandwichTypeList.Items.Add(s + String.Format(" - {0:C}", VEGGIE_PRICE));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("Sandwich type doesn't exist");
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                Messages.Text = "Sandwich type could not load";
            }
            
            // Initialilze the CheckBoxes with the ingredients
            Topping1.Content = Toppings.cheese;
            Topping2.Content = Toppings.egg;
            Topping3.Content = Toppings.lettuce;
            Topping4.Content = Toppings.onion;
            Topping5.Content = Toppings.pickles;
            Topping6.Content = Toppings.tomato;
        }

        /// <summary>
        /// Creates a meal and adds it to the order
        /// </summary>
        private void AddToOrder(object sender, RoutedEventArgs e)
        {
            if (SandwichTypeList.SelectedItem == null)
            {
                Messages.Text = "Please select a sandwich.";
            }
            else
            {
                toppings = new List<string>();
                Orderbtn.Visibility = Visibility.Visible;
                Messages.Text = String.Empty;

                // Get the sandwich type
                String[] type = SandwichTypeList.SelectedItem.ToString().Split(' ');
                sandwichType = (SandwichTypes)Enum.Parse(typeof(SandwichTypes), type[0]);

                // Get the sandwich price
                try
                {
                    switch (sandwichType)
                    {
                        case SandwichTypes.Chicken:
                            sandwichPrice = CHICKEN_PRICE;
                            break;
                        case SandwichTypes.Fish:
                            sandwichPrice = FISH_PRICE;
                            break;
                        case SandwichTypes.Turkey:
                            sandwichPrice = TURKEY_PRICE;
                            break;
                        case SandwichTypes.Veggie:
                            sandwichPrice = VEGGIE_PRICE;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("Price does not exist");
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    Messages.Text = "Sandwich price could not be calculated";
                }
                
                // Get the sandwich toppings, fries, and drinks
                foreach (CheckBox c in ToppingsList.Children.OfType<CheckBox>())
                {
                    if (c.IsChecked == true)
                    {
                        if (Enum.IsDefined(typeof(Toppings), c.Content.ToString()))
                            toppings.Add(c.Content.ToString());
                    }
                }
                if (Drink.IsChecked == true)
                    drinks = 1;

                if (Fries.IsChecked == true)
                    fries = 1;

                // Create a sandwich
                sandwichOrder = new Sandwich(sandwichType, sandwichPrice, toppings);

                // Create the order
                try
                {
                    finalOrder.addSandwich(sandwichOrder);
                    finalOrder.addDrinks(drinks);
                    finalOrder.addFries(fries);
                }
                catch (ArgumentException)
                {
                    Messages.Text = "Invalid order. Please verify that a sandwich was ordered, or that the drinks and fries are positive.";
                }
                catch (Exception)
                {
                    Messages.Text = "Invalid order. Please add a sandwich.";
                }

                // Display the current price for the order
                CurrentPrice.Text = $"\nSandwich Price: {String.Format("{0:C}", finalOrder.calculateSandwichPrices())}" +
                                    $"\nDrinks Price:      {String.Format("{0:C}", finalOrder.calculateDrinkPrices())}" +
                                    $"\nFries Price:         {String.Format("{0:C}", finalOrder.calculateFriesPrices())}" +
                                    $"\n{TAX * 100.0F}% Tax               {String.Format("{0:C}", finalOrder.calculateTax())}" +
                                    $"\n-----------------------------" +
                                    $"\nTotal: {String.Format("{0:C}", finalOrder.calculateOrderPrice() + finalOrder.calculateTax())}";
                Preview.Text += $"\n\nPrice: {String.Format("{0:C}", sandwichPrice)}" +
                                $"\n     Sandwich: {finalOrder.getSandwich(numOfOrders).getType().ToString()}" +
                                $"\n     Toppings: {finalOrder.getSandwich(numOfOrders).getToppings()}" +
                                $"\n     Fries: {fries}     Drinks: {drinks}";

                // Clear Form
                clearForm();
            }
        }

        /// <summary>
        /// Clears and resets the form for another order
        /// </summary>
        public void clearForm()
        {
            SandwichTypeList.SelectedItem = null;
            foreach (CheckBox c in ToppingsList.Children.OfType<CheckBox>())
            {
                if (c.IsChecked == true)
                {
                    c.IsChecked = false;
                }
            }

            numOfOrders++;
            fries  = 0;
            drinks = 0;
        }

        /// <summary>
        /// Finalizes the order and refreshes the form so that a new order can be placed.
        /// </summary>
        private void confirmOrder(object sender, RoutedEventArgs e)
        {
            clearForm();
            Preview.Text      = String.Empty;
            CurrentPrice.Text = String.Empty;
            Messages.Text     = "Order Completed!";
        }
    }
}
