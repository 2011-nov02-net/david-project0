using System;
using System.Collections.Generic;
using System.Linq;
using Store.DatabaseModels;
using Microsoft.EntityFrameworkCore;


namespace Store.Library
{
    public class CustomerRepository
    {

        private readonly DbContextOptions<Project0Context> _dbContext;

        /// <summary>
        /// Constructor that will set up the context of the db
        /// </summary>
        /// <param name="contextOptions">The Database Connection</param>
        public CustomerRepository(DbContextOptions<Project0Context> contextOptions)
        {
            _dbContext = contextOptions;
        }

        /// <summary>
        /// Add a location to the list of locations
        /// </summary>
        /// <param name="location">The location to be added</param>
        public void AddCustomer(string firstName, string lastName)
        {
            // get the context of the db
            using var context = new Project0Context(_dbContext);
            // create a new customer from the DatabaseModel
            if (firstName.Length > 0 && lastName.Length > 0)
            {
                DatabaseModels.Customer cust = new DatabaseModels.Customer()
                {
                    FirstName = firstName,
                    LastName = lastName
                };
                //add customer to context and save it to DB
                context.Add(cust);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Gets a new list of all customers
        /// </summary>
        /// <returns>All Customers</returns>
        public ICollection<Customer> GetAllCustomers()
        {
            // set up context
            using var context = new Project0Context(_dbContext);
            // get all the customer from db
            var dbCustomers = context.Customers.ToList();
            // convert and return to our customer class
            return dbCustomers.Select(c => new Customer(c.FirstName, c.LastName, c.Id)).ToList();
        }

        /// <summary>
        /// check to see if the id given is an actual customer
        /// </summary>
        /// <param name="id">The id we want to check</param>
        /// <returns>True if customer exists, False otherwise</returns>
        public bool IsCustomer(int id)
        {
            // set up context
            using var context = new Project0Context(_dbContext);
            return context.Customers.Any(c => c.Id == id);
        }

        /// <summary>
        /// check to see if the first and last name given is an actual customer
        /// </summary>
        /// <param name="firstName">Customer First Name to check for</param>
        /// <param name="lastName">Customer last name to check for</param>
        /// <returns>True if customer with name exists, False otherwise</returns>
        public bool IsCustomer(string firstName, string lastName)
        {
            // set up context
            using var context = new Project0Context(_dbContext);
            return context.Customers.Any(c => c.FirstName == firstName && c.LastName == lastName);
        }

        public int NumberOfCustomers()
        {
            // set up context
            using var context = new Project0Context(_dbContext);
            return context.Customers.ToList().Count();
        }

        public Customer GetCustomer(int id)
        {
            // set up context
            using var context = new Project0Context(_dbContext);
            var dbCust = context.Customers.FirstOrDefault(c => c.Id == id);
            return new Customer(dbCust.FirstName, dbCust.LastName, dbCust.Id) ?? null;
        }

        public Customer GetCustomer(string firstName, string lastName)
        {
            // set up context
            using var context = new Project0Context(_dbContext);
            var dbCust = context.Customers.FirstOrDefault(c => c.FirstName == firstName && c.LastName == lastName);
            return new Customer(dbCust.FirstName, dbCust.LastName, dbCust.Id) ?? null;
        }
    }
}
