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
            Orders = new OrderRepository(s_dbContextOptions);
        }

        // ---------------------------------------------------------------------
        // All Customer related Session Methods go here

        public void AddCustomer(string firstName, string lastName)
        {
            Customers.AddCustomer(firstName, lastName);
        }

        public List<Customer> GetAllCustomers()
        {
            return new List<Customer>(Customers.GetAllCustomers());
        }

        public void SetCurrentCustomer(int id)
        {
            CurrentCustomer = Customers.GetCustomer(id);
        }

        public string ShowCurrentCustomer()
        {
            return CurrentCustomer?.ToString() ?? "No Customer Currently Selected";
        }

        public bool IsCustomer(int id)
        {
            return Customers.IsCustomer(id);
        }

        public int NumOfCurrentCustomers()
        {
            return Customers.NumberOfCustomers();
        }

        // ---------------------------------------------------------------------
        // All Location related Session Methods go here

        public void AddLocation(string name)
        {
            Locations.AddLocation(name);
        }

        public List<Location> GetAllLocations()
        {
            return new List<Location>(Locations.GetAllLocations());
        }

        public void SetCurrentLocation(int id)
        {
            CurrentLocation = Locations.GetLocation(id);
        }

        public string ShowCurrentLocation()
        {
            return CurrentLocation?.ToString() ?? "No Location Currently Selected";
        }

        public int NumOfCurrentLocations()
        {
            return Locations.NumberOfLocations();
        }

        public bool IsLocation(int id)
        {
            return Locations.IsLocation(id);
        }

        public ICollection<Inventory> GetLocationInventory()
        {
            return Locations.GetLocationInventory(CurrentLocation);

        }

        public bool IsInLocationInventory(int id)
        {
            return Locations.IsInLocationInventory(CurrentLocation, id);
        }

        public bool AddLocationInventory(int productId, int quantity)
        {
            return Locations.AddLocationInventory(CurrentLocation, productId, quantity);
        }

        public bool AddLocationInventory(DatabaseModels.Product product, int quantity)
        {
            return Locations.AddLocationInventory(CurrentLocation, product, quantity);
        }

        public bool AddLocationNewInventory(string name, string description, decimal price, int orderLimit, int quantity)
        {
            CreateProduct(name, description, price, orderLimit);
            var product = GetProduct(name);
            return Locations.AddLocationInventory(CurrentLocation, product, quantity);
        }

        public bool RemoveLocationInventory(int productId)
        {
            return Locations.RemoveLocationInventory(CurrentLocation, productId);
        }

        // ---------------------------------------------------------------------
        // All Product related Session Methods go here

        public bool IsProduct(string name)
        {
            return Products.IsProduct(name);
        }

        public DatabaseModels.Product GetProduct(string name)
        {
            return Products.GetProduct(name);
        }

        public void CreateProduct(string name, string description, decimal price, int orderLimit)
        {
            Products.AddDbProduct(name, description, price, orderLimit);
        }



        // ---------------------------------------------------------------------
        // All Closing related Session Methods go here

        public void CloseSession()
        {
            //does nothing at the moment, but will eventually save the details of the session
        }

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
