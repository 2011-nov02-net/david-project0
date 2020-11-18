using System;
using System.Collections.Generic;
using Store.Library;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Store.DatabaseModels;
using Store.Library.Sorting;

namespace Store.ConsoleApp
{
    public class Program
    {
        // start a session
        public static Session ses = new Session();

        /// <summary>
        /// Main method, Will continously print menu, prompt user, and pass to menu selection.
        /// </summary>
        static void Main(string[] args)
        {
            bool cont = true;

            while (cont)
            {
                PrintMainMenu();
                string input = Console.ReadLine();
                MainMenuSelection(input, ref cont);
            }
        }

        /// <summary>
        /// Prints the main menu
        /// </summary>
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
            Console.WriteLine("(13) View an Order's Details");
            Console.WriteLine("E(x)it");
            Console.Write("Selection: ");
        }

        /// <summary>
        /// Handles the user input and determines which logic should be done
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cont"></param>
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
                case "13":
                    ViewOrderDetails();
                    WaitOnKeyPress();
                    break;
                case "x":
                    cont = false;
                    break;
            }
        }

        //--------------------------------------------------------------------
        // Customer Console

        /// <summary>
        /// Adds a customer to the session
        /// </summary>
        public static void AddCustomerConsole()
        {
            Console.Clear();
            Console.WriteLine("Please provide first name:");
            string firstName = Console.ReadLine();
            Console.WriteLine("Please provide last name:");
            string lastName = Console.ReadLine();
            ses.AddCustomer(firstName, lastName);
        }

        /// <summary>
        /// Prints list of all customers
        /// </summary>
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

        /// <summary>
        /// Prints list of all customers and allows user to select one
        /// </summary>
        /// <remarks>
        /// The Customer selected gets passed to the session where the data is stored
        /// Nothing is stored in the app
        /// </remarks>
        public static void GetAndSetCustomerSelection()
        {
            if (!(ses.NumOfCurrentCustomers() == 0))
            {
                string input = "";
                while(input != "id" && input != "n")
                {
                    Console.Write("Search by Customer (Id) or (n)ame: ");
                    input = Console.ReadLine().Trim().ToLower();
                }

                if (input == "id")
                    SetById();
                else
                    SetByName();
            }
            else
            {
                WaitOnKeyPress();
            }
        }

        /// <summary>
        /// Asks the user for customer id to set in the session
        /// </summary>
        public static void SetById()
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

            // set the customer
            ses.SetCurrentCustomer(input);
        }

        /// <summary>
        /// Asks the user for customer name to set in the session
        /// </summary>
        public static void SetByName()
        {
            bool isCust = false;
            string firstName = "";
            string lastName = "";

            while (!isCust)
            {
                //get the first name
                while (firstName.Length == 0)
                {
                    Console.Write("Please enter first name: ");
                    firstName = Console.ReadLine().Trim();
                }
                // get the last name  
                while (lastName.Length == 0)
                {
                    Console.Write("Please enter last name: ");
                    lastName = Console.ReadLine().Trim();
                }

                if(ses.IsCustomer(firstName, lastName))
                    isCust = true;
                else
                    Console.WriteLine("Please enter a valid customer name.");
            }

            ses.SetCurrentCustomer(firstName, lastName);
        }

        //--------------------------------------------------------------------
        // Location Console

        /// <summary>
        /// Add a new location to the session
        /// </summary>
        public static void AddLocationConsole()
        {
            Console.Clear();
            Console.WriteLine("Please provide Store Name:");
            string name = Console.ReadLine();
            ses.AddLocation(name);
        }

        /// <summary>
        /// Prints list of all locaions 
        /// </summary>
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

        /// <summary>
        /// Prints location and sets location in Session
        /// </summary>
        /// <remarks>
        /// The Location selected gets passed to the session where the data is stored
        /// Nothing is stored in the app
        /// </remarks>
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

        /// <summary>
        /// Prints inventory from the current location stored in the session
        /// </summary>
        /// <remarks>
        /// Will get a list of the current inventory from the location stored in the session
        /// </remarks>
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
                    Console.WriteLine("Product Id | Product Name | Product Description \t\t\t |   Price | Order Limit | Quantity In Stock");
                    foreach (var item in inventory)
                    {
                        var product = item.ProductObj;
                        Console.WriteLine($"{product.Id, 10} |{product.Name, 13} | {product.Description, 44} | {product.Price.ToString("F"), 7} | {product.OrderLimit,11} | {item.Quantity, 10}");
                        Console.WriteLine("--------------------------------------------------------------------------------------------------------------------");
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

        /// <summary>
        /// Gets from the user inventory to add to the session 
        /// </summary>
        /// <remarks>
        /// There is some logic here to handle input from the user regarding if it is an
        /// item that shows up on the inventory screen or is a new product to add to inventory
        /// </remarks>
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

        /// <summary>
        /// Add a quantity to existing product in inventory
        /// </summary>
        public static void AddExistingInventory()
        {
            // check to see if there is actually anything in this inventory
            if (ses.GetLocationInventory().Count != 0)
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
            else
            {
                Console.WriteLine("No inventory Found.");
            }
        }

        /// <summary>
        /// Adds a new product and inventory
        /// </summary>
        /// <remarks>
        /// Will search products to find a match before adding it to inventory
        /// If it finds a match will use that product instead of creating a new product
        /// </remarks>
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

        /// <summary>
        /// Removes a whole item from inventory
        /// </summary>
        /// <remarks>
        /// Will remove and then will not show that item again in the inventory list until it is added back in
        /// </remarks>
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

        /// <summary>
        /// View all orders, will sort order before showing
        /// </summary>
        public static void ViewAllOrders()
        {
            Console.Clear();
            // get all orders from the db
            var orders = ses.AllOrders;

            //get which sorter to implement
            ISorter sorter;
            sorter = GetSorter();

            //make sure there are orders
            if(orders.Count > 0)
            {
                foreach (var item in sorter.SortOrders(orders))
                {
                    //print each one
                    Console.WriteLine("Order Number | Date \t\t\t| Customer ID | Customer Last Name | Location ID |      Total");
                    var cust = ses.GetCustomer(item.CustomerId);
                    Console.WriteLine($"{item.OrderNumber, 12} |{item.Date, 25 } | {item.CustomerId,11} | {cust?.LastName, 19}| {item.LocationId, 11} | {item.OrderTotal.ToString("F"), 10}");
                    Console.WriteLine("-----------------------------------------------------------------------------------------------------");
                }
            }

        }

        /// <summary>
        /// Will show any orders by the current customer stored in the session
        /// </summary>
        /// <remarks>
        /// There has to be a customer selected before it will show any orders.
        /// i.e. will not ask for customer if there isn't one selected
        /// </remarks>
        public static void ViewAllOrdersByCustomer()
        {
            Console.Clear();
            if (ses.CurrentCustomer != null)
            {
                var orders = ses.AllOrdersByCustomer;

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

        /// <summary>
        /// Will show any orders at the current location stored in the session
        /// </summary>
        /// <remarks>
        /// There has to be a location selected before it will show any orders.
        /// i.e. will not ask for location if there isn't one selected
        /// </remarks>
        public static void ViewAllOrdersByLocation()
        {
            Console.Clear();
            if(ses.CurrentLocation != null)
            {
                var orders = ses.AllOrdersByLocation;

                //make sure there are orders
                if (orders.Count > 0)
                {
                    foreach (var item in orders)
                    {
                        //print each one
                        Console.WriteLine("Order Number | Date \t\t\t| Customer ID | Customer Last Name | Location ID |      Total");
                        var cust = ses.GetCustomer(item.CustomerId);
                        var total = ses.GetOrderTotal(item.OrderNumber);
                        Console.WriteLine($"{item.OrderNumber,12} |{item.Date, 25} | {item.CustomerId,11} | {cust?.LastName,19}| {item.LocationId,11} | {total.ToString("F"), 10}");
                        Console.WriteLine("-----------------------------------------------------------------------------------------------------");
                    }
                }
            }
            else
            {
                Console.WriteLine("No Orders to show from current Location.");
            }
        }

        /// <summary>
        /// Displays all the information about a particular order
        /// </summary>
        /// <remarks>
        /// Will operate on the entire list of orders, not particular customers or locations
        /// at this time. For future development.
        /// </remarks>
        public static void ViewOrderDetails()
        {

            int orderId = 0;
            while(orderId == 0)
            {
                Console.Clear();
                ViewAllOrders();
                Console.Write("Select order id of order you wish to view: ");
                string input = Console.ReadLine();
                try
                {
                    orderId = Int32.Parse(input);
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Please enter a valid number");
                    continue;
                }

                // get the order
                var order = ses.GetOrder(orderId);

                if(order != null)
                {
                    Console.Clear();
                    Console.WriteLine("Order Number | Date \t\t\t| Customer ID | Customer Last Name | Location ID |      Total");
                    var cust = ses.GetCustomer(order.CustomerId);
                    Console.WriteLine($"{order.OrderNumber,12} |{order.Date,25 } | {order.CustomerId,11} | {cust?.LastName,19}| {order.LocationId,11} | {order.OrderTotal,10}");
                    Console.WriteLine("-----------------------------------------------------------------------------------------------------");
                    Console.WriteLine("Product ID | Product Name\t | Purchased Price | Quantity Ordered");

                    foreach (var item in order.SalesList)
                    {
                        // print each item in the sale
                        Console.WriteLine($"{item.ProductId, 11}|{item.ProductName, 21}|{item.PurchasePrice, 17}|{item.SaleQuantity, 9}");
                        Console.WriteLine("---------------------------------------------------------------------");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Order Number");
                    orderId = 0;
                }
            }
        }

        /// <summary>
        /// creates the list of products for an order to have
        /// </summary>
        /// <remarks>
        /// The list of current products in the order is actually stored in the session.
        /// This is just an input/ouput to show and get from the user
        /// </remarks>
        public static void MakeOrder()
        {
            Console.Clear();
            if (ses.CurrentCustomer != null && ses.CurrentLocation != null)
            {
                string input = "";
                while (input != "d")
                {
                    // Clear the console
                    Console.Clear();
                    // Display products from the store selected
                    PrintLocationInventory();
                    PrintCurrentOrder(ses.GetCurrentOrderSales());
                    //ask the user for a product Id or if they are done
                    Console.Write("Type Product Id or (D)one:");
                    input = Console.ReadLine();
                    if (input.ToLower() == "d")
                        continue;

                    // parse the product number
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
                        // get quantity from the user
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
                            ses.AddSaleToOrder(sale);
                        }
                        else
                        {
                            // error checking
                            if(quantity <= 0)
                            {
                                Console.WriteLine("Please Enter Value above 0.");
                            }
                            else
                            {
                                // reach here when the quantity requested is higher than the order limit or doesn't have enough inventory
                                Console.WriteLine("Please enter a valid number.");
                            }
                            WaitOnKeyPress();
                        }

                    }
                }
                // when done, check to see if there was anything added to the order
                // Don't want empty orders to be added to the db
                if(ses.GetCurrentOrderSales().Count != 0)
                {
                    ses.AddOrder();
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

        /// <summary>
        /// Prints the order currently being built
        /// </summary>
        /// <param name="sales"></param>
        /// <remarks>
        /// The session keeps track of the order, this asks the session for
        /// the list and will print it out
        /// </remarks>
        public static void PrintCurrentOrder(ICollection<Library.Sale> sales)
        {
            // Print the products in the current order
            Console.WriteLine("Current Order:");
            Console.WriteLine("--------------");
            Console.WriteLine("\tProduct ID | Quantity");
            if(sales.Count != 0)
            {
                foreach(var item in sales)
                {
                    Console.WriteLine($"{item.ProductId,18} | {item.SaleQuantity,7}");
                }
            }
        }


        //--------------------------------------------------------------------
        // Miscellaneous Console

        /// <summary>
        /// Waits for key press
        /// </summary>
        public static void WaitOnKeyPress()
        {
            //wait for key press
            Console.WriteLine("Press any key to go back");
            Console.ReadKey();
        }

        /// <summary>
        /// Asks the user for which sort they would like to use
        /// </summary>
        /// <returns>The Sorter</returns>
        public static ISorter GetSorter()
        {
            ISorter sorter = new NonSorter();
            string input = "";
            while(input != "dn" && input != "do" && input != "oh" && input != "ol" && input != "ond" && input != "ona" && input != "d")
            {
                Console.WriteLine("Please Enter Sort Method");
                Console.WriteLine("(dn) Date Newest | (do) Date Oldest | (oh) Order Total Highest | (ol) Order Total Lowest");
                Console.WriteLine("(ond) Order Number Descending | (ona) Order Number Ascending | (d) Default");
                Console.Write("Selection: ");
                input = Console.ReadLine().Trim().ToLower();
            }

            Console.Clear();

            switch (input)
            {
                case "dn":
                    sorter = new NewestOrderSorter();
                    break;
                case "do":
                    sorter = new OldestOrderSorter();
                    break;
                case "oh":
                    sorter = new HighestOrderTotalSorter();
                    break;
                case "ol":
                    sorter = new LowestOrderTotalSorter();
                    break;
                case "ond":
                    sorter = new OrderNumberDescendingSorter();
                    break;
                case "ona":
                    sorter = new OrderNumberAscendingSorter();
                    break;
            }

            return sorter;
        }
    }
}
