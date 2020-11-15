using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Store.DatabaseModels;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Store.Library
{
    public class Session
    {
        //session variables
        public LocationRepository Locations { get; set; }

        public CustomerRepository Customers;
        public OrderRepository Orders { get; set; }
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
