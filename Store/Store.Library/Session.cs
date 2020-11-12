﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Library
{
    public class Session
    {
        //session variables
        public LocationRepository Locations { get; set; }
        public CustomerRepository Customers { get; set; }
        public OrderRepository Orders { get; set; }

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

        public bool SetCurrentCustomer(int id)
        {
            return false;
        }

        // ---------------------------------------------------------------------
        // All Closing related Session Methods go here

        public void CloseSession()
        {
            //does nothing at the moment, but will eventually save the details of the session
        }
    }
}