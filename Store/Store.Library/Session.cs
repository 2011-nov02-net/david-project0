using System;
using System.Collections.Generic;
using System.Text;

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

        public Session()
        {
            Locations = new LocationRepository();
            Customers = new CustomerRepository();
            Orders = new OrderRepository();
        }

        // ---------------------------------------------------------------------
        // All Customer related Session Methods go here

        public void AddCustomer(string firstName, string lastName)
        {
            Customers.AddCustomer(Customers.CreateCustomer(firstName, lastName));
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
            Locations.AddLocation(Locations.CreateLocation(name));
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

        // ---------------------------------------------------------------------
        // All Closing related Session Methods go here

        public void CloseSession()
        {
            //does nothing at the moment, but will eventually save the details of the session
        }
    }
}
