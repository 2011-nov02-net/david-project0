using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Store.Library
{
    public class CustomerRepository
    {
        private readonly ICollection<Customer> _customer;
        private static int _idCounter;

        /// <summary>
        /// Constructor that will take a preformed set of Customers and store it
        /// </summary>
        /// <param name="location"> the Collection of Customers</param>
        public CustomerRepository(ICollection<Customer> customers)
        {
            _customer = customers ?? throw new ArgumentNullException(nameof(customers));
            //set the id counter
            _idCounter = customers.Count + 1;
        }

        /// <summary>
        /// Constructor that will make an empty list of location
        /// </summary>
        public CustomerRepository()
        {
            _customer = new List<Customer>();
            _idCounter = 1;
        }

        /// <summary>
        /// Add a location to the list of locations
        /// </summary>
        /// <param name="location">The location to be added</param>
        public void AddLocation(Customer customer)
        {
            if (_customer.Any(c => c.Id == customer.Id))
            {
                throw new InvalidOperationException($"Location with ID {customer.Id} already exits.");
            }
            _customer.Add(customer);
        }

        /// <summary>
        /// Creates and returns the location object
        /// </summary>
        /// <remarks>
        /// Make the Location with this method to ensure that the id gets set
        /// sequentially
        /// </remarks>
        /// <param name="name">Name of the location</param>
        /// <returns>The Location</returns>
        public Customer CreateCustomer(string firstName, string lastName)
        {
            Customer customer = new Customer(firstName, lastName, _idCounter);
            _idCounter++;
            return customer;
        }

        /// <summary>
        /// Get and return location with a given id
        /// </summary>
        /// <param name="id">Id of the location we want</param>
        /// <returns>The Location</returns>
        public Customer GetCustomer(int id)
        {
            return _customer.First(c => c.Id == id);
        }


        public ICollection<Customer> GetAllCustomers()
        {
            return new List<Customer>(_customer);
        }

        public bool IsCustomer(int id)
        {
            return _customer.Any(c => c.Id == id);
        }
    }
}
