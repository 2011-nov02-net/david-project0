using System;
using System.Collections.Generic;
using Store.Library;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Store.DatabaseModels;

namespace Store.ConsoleApp
{
    public class Program
    {
        // start a session
        public static Session ses = new Session();

        static void Main(string[] args)
        {
            bool cont = true;

            while (cont)
            {
                PrintMainMenu();
                string input = GetUserInput();
                MainMenuSelection(input, ref cont);
            }
        }

        public static void PrintMainMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Store!");
            Console.WriteLine("Customer: " + ses.ShowCurrentCustomer());
            Console.WriteLine("Location: " + ses.ShowCurrentLocation());
            Console.WriteLine("Please Select From the Following Options: ");
            Console.WriteLine("(1) Add Customer");
            Console.WriteLine("(2) View All Customers");
            Console.WriteLine("(3) Change Customer");
            Console.WriteLine("(4) Add Location");
            Console.WriteLine("(5) View All Locations");
            Console.WriteLine("(6) Select Location");
            Console.WriteLine("(7) View Inventory at Location");
            Console.WriteLine("(8) Modify Inventory at Location");
            Console.WriteLine("(9) View All Orders");
            Console.WriteLine("(10) View All Orders By Customer");
            Console.WriteLine("(11) View All Orders At Location");
            Console.WriteLine("(12) Make New Order");
            Console.WriteLine("E(x)it");
            Console.Write("Selection: ");
        }

        public static string GetUserInput()
        {
            return Console.ReadLine();
        }

        public static void MainMenuSelection(string input, ref bool cont)
        {
            switch(input.ToLower())
            {
                case "1":
                    AddCustomerConsole();
                    break;
                case "2":
                    PrintAllCustomersConsole();
                    WaitOnKeyPress();
                    break;
                case "3":
                    PrintAllCustomersConsole();
                    GetAndSetCustomerSelection();
                    break;
                case "4":
                    AddLocationConsole();
                    break;
                case "5":
                    PrintAllLocationsConsole();
                    WaitOnKeyPress();
                    break;
                case "6":
                    PrintAllLocationsConsole();
                    GetAndSetLocationSelection();
                    break;
                case "7":
                    PrintLocationInventory();
                    WaitOnKeyPress();
                    break;
                case "8":
                    ModifyCurrentLocationInventory();
                    WaitOnKeyPress();
                    break;
                case "9":
                    ViewAllOrders();
                    WaitOnKeyPress();
                    break;
                case "10":
                    ViewAllOrdersByCustomer();
                    WaitOnKeyPress();
                    break;
                case "11":
                    ViewAllOrdersByLocation();
                    WaitOnKeyPress();
                    break;
                case "12":
                    MakeOrder();
                    WaitOnKeyPress();
                    break;
                case "x":
                    cont = false;
                    ses.CloseSession();
                    break;
            }
        }

        //--------------------------------------------------------------------
        // Customer Console

        public static void AddCustomerConsole()
        {
            Console.Clear();
            Console.WriteLine("Please provide first name:");
            string firstName = Console.ReadLine();
            Console.WriteLine("Please provide last name:");
            string lastName = Console.ReadLine();
            ses.AddCustomer(firstName, lastName);
        }

        public static void PrintAllCustomersConsole()
        {
            Console.Clear();

            if (!(ses.NumOfCurrentCustomers() == 0))
            {
                var customers = ses.GetAllCustomers();
                foreach (var cust in customers)
                {
                    Console.WriteLine("First Name\t|Last Name\t | Customer ID");
                    Console.WriteLine($"{cust.FirstName,16}|{cust.LastName,16}| {cust.Id}");
                    Console.WriteLine("----------------------------------------------");
                }
            }
            else
            {
                Console.WriteLine("No Customers Currently in System.");
            }
        }

        public static void GetAndSetCustomerSelection()
        {
            if (!(ses.NumOfCurrentCustomers() == 0))
            {
                //since we have already displayed all customers we can just get the selection here
                int input = -1;
                while (input <= 0)
                {
                    Console.Write("Please enter your Customer Id: ");
                    try
                    {
                        input = Int32.Parse(Console.ReadLine());
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please Enter a valid number");
                        continue;
                    }

                    //check to see if the id given is an actual customer
                    if (!ses.IsCustomer(input))
                    {
                        Console.WriteLine("Please enter a valid Customer ID");
                        input = -1;
                    }
                }
                // have a customer id, tell the session to remember that
                ses.SetCurrentCustomer(input);
            }
            else
            {
                WaitOnKeyPress();
            }
        }

        //--------------------------------------------------------------------
        // Location Console

        public static void AddLocationConsole()
        {
            Console.Clear();
            Console.WriteLine("Please provide Store Name:");
            string name = Console.ReadLine();
            ses.AddLocation(name);
        }

        public static void PrintAllLocationsConsole()
        {
            if (!(ses.NumOfCurrentLocations() == 0))
            {
                var locations = ses.GetAllLocations();

                Console.Clear();

                foreach (var location in locations)
                {
                    Console.WriteLine("Store Name\t| Location ID");
                    Console.WriteLine($"{location.Name, -16}| {location.Id}");
                    Console.WriteLine("------------------------------");
                }
            }
            else
            {
                Console.WriteLine("No Locations Currently in System.");
            }    
        }

        public static void GetAndSetLocationSelection()
        {
            if (ses.NumOfCurrentLocations() > 0)
            {
                //since we have already displayed all customers we can just get the selection here
                int input = -1;
                while (input <= 0)
                {
                    Console.Write("Please enter the Location Id: ");
                    try
                    {
                        input = Int32.Parse(Console.ReadLine());
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please Enter a valid number");
                        continue;
                    }

                    //check to see if the id given is an actual customer
                    if (!ses.IsLocation(input))
                    {
                        Console.WriteLine("Please enter a valid Location ID");
                        input = -1;
                    }
                }
                // have a customer id, tell the session to remember that
                ses.SetCurrentLocation(input);
            }
            else
            {
                WaitOnKeyPress();
            }
        }

        public static void PrintLocationInventory()
        {
            Console.Clear();
            // check to see if there is a location selected
            if (!(ses.CurrentLocation == null))
            {
                // get the inventory from the session
                var inventory = ses.GetLocationInventory();
                // make sure that the store has inventory
                if (inventory.Count > 0)
                {
                    foreach (var item in inventory)
                    {
                        var product = item.ProductObj;
                        Console.WriteLine($"{product.Id}: {product.Name} | {product.Description} | {product.Price}");
                        Console.WriteLine($"Quantity: {item.Quantity}");
                    }
                }
                else
                {
                    Console.WriteLine("No inventory currently in this store");
                }
            }
            else
            {
                Console.WriteLine("No store currently selected, Please select a store.");
            }
        }

        public static void ModifyCurrentLocationInventory()
        {
            // check to see if we have a location selected
            if (ses.CurrentLocation != null)
            {
                Console.Clear();
                PrintLocationInventory();
                // ask if user wants to add to previous inventory or create new item\
                string input = null;
                while (input != "a" && input != "c" && input != "r")
                {
                    Console.Write("(A)dd/(R)emove existing product or (C)reate new product: ");
                    input = Console.ReadLine().ToLower();

                }

                if (input == "a")
                {
                    AddExistingInventory();
                }
                else if (input == "c")
                {
                    AddNewInventory();
                }
                else
                {
                    RemoveExisitingInventory();
                }
            }
            else
            {
                Console.WriteLine("No Store Selected.");
            }

        }

        public static void AddExistingInventory()
        {
            //since we have already displayed all the inventory at the location we can just get the selection here
            int productInput = -1;
            while (productInput <= 0)
            {
                Console.Clear();
                PrintLocationInventory();
                Console.Write("Please enter the Product Id to add inventory to: ");
                try
                {
                    productInput = Int32.Parse(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please Enter a valid number");
                    continue;
                }

                //check to see if the id given is an actual customer
                if (!ses.IsInLocationInventory(productInput))
                {
                    Console.WriteLine("Please enter a valid Product ID");
                    productInput = -1;
                }
            }

            int quantityToAdd = -1;
            while (quantityToAdd <= 0)
            {
                Console.Write("Quantity to add: ");
                try
                {
                    quantityToAdd = Int32.Parse(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please Enter a valid number");
                    continue;
                }
            }

            if (ses.AddLocationInventory(productInput, quantityToAdd))
            {
                Console.WriteLine($"{quantityToAdd} added to inventory!");
            }
            else
            {
                Console.WriteLine("Error adding to inventory");
            }
        }

        public static void AddNewInventory()
        {
            Console.Clear();
            //get name of item to add
            string name = "";

            while(name.Length == 0)
            {
                Console.Write("Name of Product: ");
                name = Console.ReadLine();
            }

            if(ses.IsProduct(name))
            {
                Console.WriteLine("Existing Product!");
                // is an existing item
                var product = ses.GetProduct(name);
                //get the quantity
                int quantityToAdd = -1;
                while (quantityToAdd <= 0)
                {
                    Console.Write($"Quantity to add to {product.Name}: ");
                    try
                    {
                        quantityToAdd = Int32.Parse(Console.ReadLine());
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please Enter a valid number");
                        continue;
                    }
                }

                if (ses.AddLocationInventory(product, quantityToAdd))
                {
                    Console.WriteLine($"{quantityToAdd} added to inventory!");
                }
                else
                {
                    Console.WriteLine("Error adding to inventory");
                }
            }
            else
            {
                // not already a product, gather information

                // product description
                string description = "";
                while(description.Length <= 0)
                {
                    Console.Write("Product Description: ");
                    description = Console.ReadLine();
                }

                // product price
                decimal price = -1.0m;
                while(price <= 0.0m)
                {
                    Console.Write($"Price of {name}: ");
                    try
                    {
                        price = Decimal.Parse(Console.ReadLine());
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please Enter a valid number");
                        continue;
                    }
                }

                // order Limit
                int orderLimit = -1;
                while(orderLimit < 0)
                {
                    Console.Write($"Order Limit of {name}: ");
                    try
                    {
                       orderLimit = Int32.Parse(Console.ReadLine());
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please Enter a valid number");
                        continue;
                    }
                }

                int quantity = -1;
                while (quantity < 0)
                {
                    Console.Write($"Quantity of {name}: ");
                    try
                    {
                        quantity = Int32.Parse(Console.ReadLine());
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please Enter a valid number");
                        continue;
                    }
                }

                if (ses.AddLocationNewInventory(name, description, price, orderLimit, quantity))
                {
                    Console.WriteLine($"{quantity}: {name} added to location inventory");
                }
                else
                {
                    Console.WriteLine("Error adding to inventory");
                }
            }



        }

        public static void RemoveExisitingInventory()
        {
            int productInput = -1;
            while (productInput <= 0)
            {
                Console.Clear();
                PrintLocationInventory();
                Console.Write("Please enter the Product Id to remove inventory: ");
                try
                {
                    productInput = Int32.Parse(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please Enter a valid number");
                    continue;
                }

                //check to see if the id given is an actual customer
                if (!ses.IsInLocationInventory(productInput))
                {
                    Console.WriteLine("Please enter a valid Product ID");
                    productInput = -1;
                }
            }

            if (ses.RemoveLocationInventory(productInput))
            {
                Console.WriteLine("Product Removed from Inventory.");
            }
            else
            {
                Console.WriteLine("Error Removing Product from Inventory");
            }    
        }

        //--------------------------------------------------------------------
        // Orders Console

        public static void ViewAllOrders()
        {

            Console.Clear();
            // get all orders from the db
            var orders = ses.GetAllOrders();

            //make sure there are orders
            if(orders.Count > 0)
            {
                foreach (var item in orders)
                {
                    //print each one
                    Console.WriteLine("Order Number | Date \t\t\t| Customer ID | Customer Last Name | Location ID |      Total");
                    var cust = ses.GetCustomer(item.CustomerId);
                    var total = ses.GetOrderTotal(item.OrderNumber);
                    Console.WriteLine($"{item.OrderNumber, 12} |    {item.Date} | {item.CustomerId,11} | {cust?.LastName, 19}| {item.LocationId, 11} | {total.ToString("F"), 10}");
                    Console.WriteLine("-----------------------------------------------------------------------------------------------------");
                }
            }

        }

        public static void ViewAllOrdersByCustomer()
        {
            Console.Clear();
            if (ses.CurrentCustomer != null)
            {
                var orders = ses.GetAllOrdersByCustomer();

                //make sure there are orders
                if (orders.Count > 0)
                {
                    Console.WriteLine($"Showing Orders for: " + ses.ShowCurrentCustomer());
                    foreach (var item in orders)
                    {
                        //print each one
                        Console.WriteLine("Order Number | Date \t\t\t| Customer ID | Location ID |      Total");
                        var total = ses.GetOrderTotal(item.OrderNumber);
                        Console.WriteLine($"{item.OrderNumber,12} |    {item.Date} | {item.CustomerId,11} | {item.LocationId,11} | {total.ToString("F"),10}");
                        Console.WriteLine("--------------------------------------------------------------------------------");
                    }
                }
            }
            else
            {
                Console.WriteLine("No Customer Selected to show Orders.");
            }
        }

        public static void ViewAllOrdersByLocation()
        {
            Console.Clear();
            if(ses.CurrentLocation != null)
            {
                var orders = ses.GetAllOrdersByLocation();

                //make sure there are orders
                if (orders.Count > 0)
                {
                    foreach (var item in orders)
                    {
                        //print each one
                        Console.WriteLine("Order Number | Date \t\t\t| Customer ID | Customer Last Name | Location ID |      Total");
                        var cust = ses.GetCustomer(item.CustomerId);
                        var total = ses.GetOrderTotal(item.OrderNumber);
                        Console.WriteLine($"{item.OrderNumber,12} |    {item.Date} | {item.CustomerId,11} | {cust?.LastName,19}| {item.LocationId,11} | {total.ToString("F"), 10}");
                        Console.WriteLine("-----------------------------------------------------------------------------------------------------");
                    }
                }
            }
            else
            {
                Console.WriteLine("No Orders to show from current Location.");
            }
        }

        public static void MakeOrder()
        {
            Console.Clear();
            if (ses.CurrentCustomer != null && ses.CurrentLocation != null)
            {
                string input = "";
                // make a new list of products that the order will contain
                var salesList = new List<Library.Sale>();

                while (input != "d")
                {
                    // Clear the console
                    Console.Clear();
                    // Display products from the store selected
                    PrintLocationInventory();
                    PrintCurrentOrder(salesList);
                    Console.Write("Type Product Id or (D)one:");
                    input = Console.ReadLine();
                    if (input.ToLower() == "d")
                        continue;

                    int productId;
                    try
                    {
                        productId = Int32.Parse(input);
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("Please enter a number.");
                        WaitOnKeyPress();
                        continue;
                    }

                    // make sure the product id selected is in current list
                    if (ses.IsInLocationInventory(productId))
                    {
                        Console.Write("Please enter quanity: ");
                        int quantity;
                        try
                        {
                            quantity = Int32.Parse(Console.ReadLine());
                        }
                        catch (ArgumentException)
                        {
                            Console.WriteLine("Please enter a number");
                            WaitOnKeyPress();
                            continue;
                        }
                        // ensure quantity typed is above 0 and at or below order limit
                        if (quantity > 0 && ses.IsWithinOrderLimit(productId, quantity) && ses.IsEnoughInventory(productId, quantity))
                        {
                            //create new product and add it to the list
                            var sale = new Library.Sale(productId, quantity);
                            salesList.Add(sale);
                        }
                        else
                        {
                            if(quantity <= 0)
                            {
                                Console.WriteLine("Please Enter Value above 0.");
                            }
                            else
                            {
                                // reach here when the quantity requested is higher than the order limit
                                Console.WriteLine("Please enter a Value below the order Limit.");
                            }
                            WaitOnKeyPress();
                        }

                    }
                }
                if(salesList.Count != 0)
                {
                    ses.AddOrder(salesList);
                }
                else
                {
                    Console.WriteLine("Nothing added to order.");
                }
            }
            else
            {
                Console.WriteLine("Please select both a Customer and Location.");
            }
        }

        public static void PrintCurrentOrder(ICollection<Library.Sale> sales)
        {
            // Print the products in the current order
            Console.WriteLine("\tProduct | Quantity");
            if(sales.Count != 0)
            {
                foreach(var item in sales)
                {
                    Console.WriteLine($"{item.ProductId,11} | {item.SaleQuantity}");
                }
            }
        }


        //--------------------------------------------------------------------
        // Miscellaneous Console
        public static void WaitOnKeyPress()
        {
            //wait for key press
            Console.WriteLine("Press any key to go back");
            Console.ReadKey();
        }
    }
}
