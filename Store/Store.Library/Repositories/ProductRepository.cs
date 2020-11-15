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

        public bool IsProduct(string name)
        {
            // get the context of the db
            using var context = new Project0Context(_dbContext);
            return context.Products.Any(p => p.Name == name);
        }

        public DatabaseModels.Product GetProduct(string name)
        {
            // get the context of the db
            using var context = new Project0Context(_dbContext);
            return context.Products.FirstOrDefault(p => p.Name == name);
        }

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
    }
}
