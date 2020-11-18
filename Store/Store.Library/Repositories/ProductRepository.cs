using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Store.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace Store.Library.Repositories
{
    public class ProductRepository
    {
        private readonly DbContextOptions<Project0Context> _dbContext;

        public ProductRepository(DbContextOptions<Project0Context> contextOptions)
        {
            _dbContext = contextOptions;
        }

        /// <summary>
        /// Checks the database to see if the name given is already a product
        /// </summary>
        /// <param name="name">Name of the Product</param>
        /// <returns>true if product name exists, false otherwise</returns>
        public bool IsProduct(string name)
        {
            // get the context of the db
            using var context = new Project0Context(_dbContext);
            return context.Products.Any(p => p.Name == name);
        }

        /// <summary>
        /// The product object
        /// </summary>
        /// <remarks>
        /// Returns the database model so we don't have to convert back and forth
        /// </remarks>
        /// <param name="name">Name of product</param>
        /// <returns>Database Product</returns>
        public DatabaseModels.Product GetProduct(string name)
        {
            // get the context of the db
            using var context = new Project0Context(_dbContext);
            return context.Products.FirstOrDefault(p => p.Name == name);
        }

        /// <summary>
        /// Creates a new product and adds it to the database
        /// </summary>
        /// <param name="name">Name of product</param>
        /// <param name="description">Description of product</param>
        /// <param name="price">Price of product</param>
        /// <param name="orderLimit">Order Limit of product</param>
        public void AddDbProduct(string name, string description, decimal price, int orderLimit)
        {
            // get the context of the db
            using var context = new Project0Context(_dbContext);

            // Create the new product
            var product = new DatabaseModels.Product()
            {
                Name = name,
                Description = description,
                Price = price,
                OrderLimit = orderLimit
            };

            // Add to db
            context.Products.Add(product);
            context.SaveChanges();
        }

        /// <summary>
        /// Checks quantity requested to make sure that it falls within the order limit of the product
        /// </summary>
        /// <param name="id">Id of the product</param>
        /// <param name="quantity">Amount requested</param>
        /// <returns>True if within order limit, False if not</returns>
        public bool IsWithinOrderLimit(int id, int quantity)
        {
            // get the context of the db
            using var context = new Project0Context(_dbContext);

            return quantity <= context.Products.First(p => p.Id == id).OrderLimit;
        }
    }
}
