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

        public void AddCustomer(string firstName, string lastName) => Customers.AddCustomer(firstName, lastName);

        public List<Customer> GetAllCustomers() => new List<Customer>(Customers.GetAllCustomers());

        public void SetCurrentCustomer(int id) => CurrentCustomer = Customers.GetCustomer(id);

        public string ShowCurrentCustomer() => CurrentCustomer?.ToString() ?? "No Customer Currently Selected";

        public bool IsCustomer(int id) => Customers.IsCustomer(id);

        public int NumOfCurrentCustomers() => Customers.NumberOfCustomers();

        public Customer GetCustomer(int id) => Customers.GetCustomer(id);

        // ---------------------------------------------------------------------
        // All Location related Session Methods go here

        public void AddLocation(string name) => Locations.AddLocation(name);

        public List<Location> GetAllLocations() => new List<Location>(Locations.GetAllLocations());

        public void SetCurrentLocation(int id) => CurrentLocation = Locations.GetLocation(id);

        public string ShowCurrentLocation() => CurrentLocation?.ToString() ?? "No Location Currently Selected";

        public int NumOfCurrentLocations() => Locations.NumberOfLocations();

        public bool IsLocation(int id) => Locations.IsLocation(id);

        public ICollection<Inventory> GetLocationInventory() => Locations.GetLocationInventory(CurrentLocation);

        public bool IsInLocationInventory(int id) => Locations.IsInLocationInventory(CurrentLocation, id);

        public bool AddLocationInventory(int productId, int quantity) => Locations.AddLocationInventory(CurrentLocation, productId, quantity);

        public bool AddLocationInventory(DatabaseModels.Product product, int quantity) => Locations.AddLocationInventory(CurrentLocation, product, quantity);

        public bool AddLocationNewInventory(string name, string description, decimal price, int orderLimit, int quantity)
        {
            CreateProduct(name, description, price, orderLimit);
            var product = GetProduct(name);
            return Locations.AddLocationInventory(CurrentLocation, product, quantity);
        }

        public bool RemoveLocationInventory(int productId) => Locations.RemoveLocationInventory(CurrentLocation, productId);

        public bool IsEnoughInventory(int productId, int quantity) => Locations.IsEnoughInventory(CurrentLocation, productId, quantity);

        // ---------------------------------------------------------------------
        // All Product related Session Methods go here

        public bool IsProduct(string name) => Products.IsProduct(name);

        public DatabaseModels.Product GetProduct(string name) => Products.GetProduct(name);

        public void CreateProduct(string name, string description, decimal price, int orderLimit) => Products.AddDbProduct(name, description, price, orderLimit);

        public bool IsWithinOrderLimit(int productId, int quantity) => Products.IsWithinOrderLimit(productId, quantity);

        // ---------------------------------------------------------------------
        // All Order related Session Methods go here

        public List<Order> AllOrders => Orders.GetAllOrders();

        public List<Order> AllOrdersByCustomer => Orders.GetAllOrdersByCustomer(CurrentCustomer.Id);

        public List<Order> AllOrdersByLocation => Orders.GetAllOrdersByLocation(CurrentLocation.Id);

        public decimal GetOrderTotal(int orderId) => Orders.GetOrderTotal(orderId);

        public void AddOrder()
        {
            Orders.AddOrder(CurrentCustomer, CurrentLocation, SalesList);
            // clear out the sales list
            SalesList = new List<Sale>();
        }

        public Order GetOrder(int orderId) => Orders.GetOrder(orderId);

        public void AddSaleToOrder(Sale sale) => SalesList.Add(sale);

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
