using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Store.DatabaseModels;
using Newtonsoft.Json;
using System.Diagnostics;
using Store.Library.Repositories;

namespace Store.Library
{
    public class Session
    {
        //session variables
        public LocationRepository Locations { get; set; }
        public CustomerRepository Customers;
        public OrderRepository Orders { get; set; }
        public ProductRepository Products { get; set; }
        public Customer CurrentCustomer { get; set; } = null;
        public Location CurrentLocation { get; set; } = null;
        public List<Sale> SalesList { get; set; } = new List<Sale>();

        static DbContextOptions<Project0Context> s_dbContextOptions;

        public Session()
        {
            // set up the logger
            //using var logStream = new StreamWriter("Store-Logs.txt");
            // create the database connection
            var optionsBuilder = new DbContextOptionsBuilder<Project0Context>();
            optionsBuilder.UseSqlServer(GetConnectionString());
            //set up logging
            //optionsBuilder.LogTo(x => Debug.WriteLine(x), LogLevel.Error);
            //optionsBuilder.LogTo(logStream.WriteLine, LogLevel.Information);

            s_dbContextOptions = optionsBuilder.Options;

            Locations = new LocationRepository(s_dbContextOptions);
            Customers = new CustomerRepository(s_dbContextOptions);
            Products = new ProductRepository(s_dbContextOptions);
            Orders = new OrderRepository(s_dbContextOptions);
        }

        // ---------------------------------------------------------------------
        // All Customer related Session Methods go here

        /// <summary>
        /// Adds a customer
        /// </summary>
        /// <param name="firstName">First Name</param>
        /// <param name="lastName">Last Name</param>
        public void AddCustomer(string firstName, string lastName) => Customers.AddCustomer(firstName, lastName);

        /// <summary>
        /// Gets all customers from the Customer Repository
        /// </summary>
        /// <returns>All Customers</returns>
        public List<Customer> GetAllCustomers() => new List<Customer>(Customers.GetAllCustomers());

        /// <summary>
        /// Sets the Current Customer for the Session
        /// </summary>
        /// <param name="id">Customer id</param>
        public void SetCurrentCustomer(int id) => CurrentCustomer = Customers.GetCustomer(id);

        /// <summary>
        /// Sets the Current Customer for the Session
        /// </summary>
        /// <param name="firstName">First Name</param>
        /// <param name="lastName">Last Name</param>
        public void SetCurrentCustomer(string firstName, string lastName) => CurrentCustomer = Customers.GetCustomer(firstName, lastName);

        /// <summary>
        /// Returns, as a string, current customer
        /// </summary>
        /// <returns>Customer</returns>
        public string ShowCurrentCustomer() => CurrentCustomer?.ToString() ?? "No Customer Currently Selected";

        /// <summary>
        /// Checks Customer Repository to see if the given id is a customer
        /// </summary>
        /// <param name="id">Customer id</param>
        /// <returns>True if customer, false otherwise</returns>
        public bool IsCustomer(int id) => Customers.IsCustomer(id);

        /// <summary>
        /// Checks Customer Repository to see if the given name is a customer
        /// </summary>
        /// <param name="firstName">Customer first name</param>
        /// <param name="lastName">Customer last name</param>
        /// <returns>True if customer, false otherwise</returns>
        public bool IsCustomer(string firstName, string lastName) => Customers.IsCustomer(firstName, lastName);

        /// <summary>
        /// Current number of customers in the Customer Repository
        /// </summary>
        /// <returns></returns>
        public int NumOfCurrentCustomers() => Customers.NumberOfCustomers();

        /// <summary>
        /// Get Customer object from the Customer Repository
        /// </summary>
        /// <param name="id">Customer Id</param>
        /// <returns>The Customer</returns>
        public Customer GetCustomer(int id) => Customers.GetCustomer(id);

        // ---------------------------------------------------------------------
        // All Location related Session Methods go here

        /// <summary>
        /// Adds a location
        /// </summary>
        /// <param name="name">Location Name</param>
        public void AddLocation(string name) => Locations.AddLocation(name);

        /// <summary>
        /// All locations from the Location Repository
        /// </summary>
        /// <returns>All Locations</returns>
        public List<Location> GetAllLocations() => new List<Location>(Locations.GetAllLocations());

        /// <summary>
        /// Sets the Current Location for the Session
        /// </summary>
        /// <param name="id">Location id</param>
        public void SetCurrentLocation(int id) => CurrentLocation = Locations.GetLocation(id);

        /// <summary>
        /// Returns, as a string, current customer
        /// </summary>
        /// <returns>Location String</returns>
        public string ShowCurrentLocation() => CurrentLocation?.ToString() ?? "No Location Currently Selected";

        /// <summary>
        /// Current Number of locations in Location Repository
        /// </summary>
        /// <returns>Number of Locations</returns>
        public int NumOfCurrentLocations() => Locations.NumberOfLocations();

        /// <summary>
        /// Checks the Location Repository to see if given Id is a location
        /// </summary>
        /// <param name="id">Location Id to search</param>
        /// <returns>True if location, false otherwise</returns>
        public bool IsLocation(int id) => Locations.IsLocation(id);

        /// <summary>
        /// Inventory at current session Location
        /// </summary>
        /// <returns>List of Inventory at Location</returns>
        public ICollection<Inventory> GetLocationInventory() => Locations.GetLocationInventory(CurrentLocation);

        /// <summary>
        /// Checks to see if a product is in the current session Locaion
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <returns>True if in inventory, false otherwise</returns>
        public bool IsInLocationInventory(int id) => Locations.IsInLocationInventory(CurrentLocation, id);

        /// <summary>
        /// Adds inventory to a Location
        /// </summary>
        /// <param name="productId">The Product Id</param>
        /// <param name="quantity">Amount to add</param>
        /// <returns>True if sucessful, false otherwise</returns>
        public bool AddLocationInventory(int productId, int quantity) => Locations.AddLocationInventory(CurrentLocation, productId, quantity);

        /// <summary>
        /// Adds Inventory to a Location
        /// </summary>
        /// <param name="product">The Product</param>
        /// <param name="quantity">Amount to add</param>
        /// <returns>True if sucessful, false otherwise</returns>
        public bool AddLocationInventory(DatabaseModels.Product product, int quantity) => Locations.AddLocationInventory(CurrentLocation, product, quantity);

        /// <summary>
        /// Adds Inventory to a Location
        /// </summary>
        /// <remarks>
        /// This adds a new product that is formed, so a product that was not in the system beforehand
        /// </remarks>
        /// <param name="name">The Product name</param>
        /// <param name="description">The Product description</param>
        /// <param name="price">The Product price</param>
        /// <param name="orderLimit">The Product order limit</param>
        /// <param name="quantity">Amount to add</param>
        /// <returns>True if sucessful, false otherwise</returns>
        public bool AddLocationNewInventory(string name, string description, decimal price, int orderLimit, int quantity)
        {
            CreateProduct(name, description, price, orderLimit);
            var product = GetProduct(name);
            return Locations.AddLocationInventory(CurrentLocation, product, quantity);
        }

        /// <summary>
        /// Removes a while product from inventory
        /// </summary>
        /// <param name="productId">Product Id to remover</param>
        /// <returns>True if sucessful, false otherwise</returns>
        public bool RemoveLocationInventory(int productId) => Locations.RemoveLocationInventory(CurrentLocation, productId);

        /// <summary>
        /// Checks Location Repository to see if there is enough invetory to fullfil an order
        /// </summary>
        /// <param name="productId">The Product Id</param>
        /// <param name="quantity">Amount requested</param>
        /// <returns>True if enough, false otherwise</returns>
        public bool IsEnoughInventory(int productId, int quantity) => Locations.IsEnoughInventory(CurrentLocation, productId, quantity);

        // ---------------------------------------------------------------------
        // All Product related Session Methods go here

        /// <summary>
        /// Checks the Product Repository for an exisiting item
        /// </summary>
        /// <param name="name">Name of product</param>
        /// <returns>True if it is an existing product, false otherwise</returns>
        public bool IsProduct(string name) => Products.IsProduct(name);

        /// <summary>
        /// Gets the db version of the product
        /// </summary>
        /// <param name="name">product name</param>
        /// <returns>DatabaseModels Product</returns>
        public DatabaseModels.Product GetProduct(string name) => Products.GetProduct(name);

        /// <summary>
        /// Create new Product and adds to database
        /// </summary>
        /// <param name="name">Product Name</param>
        /// <param name="description">Product Description</param>
        /// <param name="price">Product Price</param>
        /// <param name="orderLimit">Product Order Limit</param>
        public void CreateProduct(string name, string description, decimal price, int orderLimit) => Products.AddDbProduct(name, description, price, orderLimit);

        /// <summary>
        /// Checks the quantity requested to the order limit of the product
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <param name="quantity">Amount Requested</param>
        /// <returns>True if within order limit, false if not</returns>
        public bool IsWithinOrderLimit(int productId, int quantity) => Products.IsWithinOrderLimit(productId, quantity);

        // ---------------------------------------------------------------------
        // All Order related Session Methods go here

        /// <summary>
        /// Gets all orders
        /// </summary>
        /// <returns>List of All Orders</returns>
        public List<Order> AllOrders => Orders.GetAllOrders();

        /// <summary>
        /// All Orders by Current Session Customer
        /// </summary>
        /// <returns>List of All Customer Orders</returns>
        public List<Order> AllOrdersByCustomer => Orders.GetAllOrdersByCustomer(CurrentCustomer.Id);

        /// <summary>
        /// All Orders by Current Session Location
        /// </summary>
        /// <returns>List of All Location Orders</returns>
        public List<Order> AllOrdersByLocation => Orders.GetAllOrdersByLocation(CurrentLocation.Id);

        /// <summary>
        /// Gets total of the order
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <returns>Total of Order</returns>
        public decimal GetOrderTotal(int orderId) => Orders.GetOrderTotal(orderId);

        /// <summary>
        /// Adds the Order to the Order Repository
        /// </summary>
        /// <remarks>
        /// Clears out sales list after order submitted so it is ready for the next order
        /// </remarks>
        public void AddOrder()
        {
            Orders.AddOrder(CurrentCustomer, CurrentLocation, SalesList);
            // clear out the sales list
            SalesList = new List<Sale>();
        }

        /// <summary>
        /// Get an order by Id
        /// </summary>
        /// <remarks>
        /// Used to get the whole order including sales list
        /// For display purposes
        /// </remarks>
        /// <param name="orderId">Order Number</param>
        /// <returns>The order</returns>
        public Order GetOrder(int orderId) => Orders.GetOrder(orderId);
        /// <summary>
        /// Adds a new item to the current order before submitting it to the Order Repository
        /// </summary>
        /// <param name="sale">Sale item</param>
        public void AddSaleToOrder(Sale sale) => SalesList.Add(sale);

        /// <summary>
        /// All sales currently in order
        /// </summary>
        /// <returns>List of Sales</returns>
        public List<Sale> GetCurrentOrderSales() => new List<Sale>(SalesList);

        // ----------------------------------------------------------------------
        // All DB connection stuff
        static string GetConnectionString()
        {
            string path = "../../../../../../Project0Connection.json";
            string json;
            try
            {
                json = File.ReadAllText(path);
            }
            catch (IOException)
            {
                Console.WriteLine($"required file {path} not found. should just be the connection string in quotes.");
                throw;
            }
            string connectionString = JsonConvert.DeserializeObject<string>(json);
            return connectionString;
        }
    }
}
