using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Store.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace Store.Library
{
    public class OrderRepository
    {

        private readonly DbContextOptions<Project0Context> _dbContext;

        public OrderRepository(DbContextOptions<Project0Context> contextOptions)
        {
            _dbContext = contextOptions;
        }

        /// <summary>
        /// Adds an order to the database
        /// </summary>
        /// <param name="customer">Customer that placed order</param>
        /// <param name="location">Location that was ordered from</param>
        /// <param name="sales">List of sales that the customer placed</param>
        public void AddOrder(Customer customer, Location location, ICollection<Sale> sales)
        {
            // get the context of the db
            using var context = new Project0Context(_dbContext);

            // create list converting from Library.Sale to DatabaseModel.Sale
            var dbSales = new List<DatabaseModels.Sale>();
            decimal orderTotal = 0.0m;

            foreach(var item in sales)
            {
                // need the product details
                var dbProduct = context.Products.First(p => p.Id == item.ProductId);
                var dbSale = new DatabaseModels.Sale()
                {
                    ProductId = item.ProductId,
                    ProductName = dbProduct.Name,
                    PurchasePrice = dbProduct.Price,
                    Quantity = item.SaleQuantity,
                };
                // add to the sum of the order total
                orderTotal += item.SaleQuantity * dbProduct.Price;

                // add the sale to the running list
                dbSales.Add(dbSale);

                // remove the amount from the inventory of the store
                var locationInventory = context.Inventories.First(i => i.LocationId == location.Id && i.ProductId == item.ProductId);
                locationInventory.Quantity -= item.SaleQuantity;
                context.Inventories.Update(locationInventory);
                context.SaveChanges();
            }

            // create the classes
            var order = new DatabaseModels.Order()
            {
                CustomerId = customer.Id,
                LocationId = location.Id,
                Date = DateTime.Now,
                Sales = dbSales
            };

            // calculate total
            order.OrderTotal = orderTotal;
            context.Orders.Add(order);
            context.SaveChanges();
        }

        /// <summary>
        /// All the orders from the DB
        /// </summary>
        /// <remarks>
        /// Converts from DatabaseModel.Order to Library.Order
        /// </remarks>
        /// <returns>List of all orders</returns>
        public List<Order> GetAllOrders()
        {
            // get the context of the db
            using var context = new Project0Context(_dbContext);

            var dbOrders = context.Orders.ToList();

            return dbOrders.Select(o => new Order(o.CustomerId, o.LocationId, o.Date, o.OrderNumber, o.OrderTotal)).ToList();
        }

        /// <summary>
        /// All the orders for a given customer from the DB
        /// </summary>
        /// <remarks>
        /// Converts from DatabaseModel.Order to Library.Order
        /// </remarks>
        /// <returns>List of all orders for a given customer</returns>
        public List<Order> GetAllOrdersByCustomer(int custId)
        {
            // get the context of the db
            using var context = new Project0Context(_dbContext);

            //get all orders where the custid of the order is the same as the one we are looking for
            var dbCustOrders = context.Orders.Where(o => o.CustomerId == custId).ToList();
            return dbCustOrders.Select(o => new Order(o.CustomerId, o.LocationId, o.Date, o.OrderNumber, o.OrderTotal)).ToList();
        }

        /// <summary>
        /// All the orders from the DB for a given location
        /// </summary>
        /// <remarks>
        /// Converts from DatabaseModel.Order to Library.Order
        /// </remarks>
        /// <returns>List of all orders for a location</returns>
        public List<Order> GetAllOrdersByLocation(int locId)
        {
            // get the context of the db
            using var context = new Project0Context(_dbContext);

            //get all orders where the locationid of the order is the same as the one we are looking for
            var dbLocationOrders = context.Orders.Where(o => o.LocationId == locId).ToList();
            return dbLocationOrders.Select(o => new Order(o.CustomerId, o.LocationId, o.Date, o.OrderNumber, o.OrderTotal)).ToList();
        }

        /// <summary>
        /// Gets total order price
        /// </summary>
        /// <param name="orderId">The order that we want to total</param>
        /// <returns>Sum of all sales in order</returns>
        public decimal GetOrderTotal(int orderId)
        {
            // get the context of the db
            using var context = new Project0Context(_dbContext);

            var list = context.Sales.Where(s => s.OrderNumber == orderId).ToList();

            decimal sum = 0;

            foreach(var item in list)
            {
                sum += item.PurchasePrice * item.Quantity;
            }

            return sum;
        }

        /// <summary>
        /// Gets one order from the database
        /// </summary>
        /// <remarks>
        /// Converts from DatabaseModel.Order to Library.Order
        /// </remarks>
        /// <param name="orderId">Order Number to get</param>
        /// <returns>The order</returns>
        public Order GetOrder(int orderId)
        {
            // get the context of the db
            using var context = new Project0Context(_dbContext);

            // get the order and convert it
            var dbOrder = context.Orders.FirstOrDefault(o => o.OrderNumber == orderId);

            // get all the sales related to the order
            var dbSales = context.Sales.Where(s => s.OrderNumber == orderId).ToList();

            // convert all sales
            var sales = new List<Sale>();

            foreach(var item in dbSales)
            {
                sales.Add(new Sale(item.ProductId, item.ProductName, item.PurchasePrice, item.Quantity));
            }

            return new Order(dbOrder.CustomerId, dbOrder.LocationId, sales, dbOrder.Date, dbOrder.OrderNumber, dbOrder.OrderTotal);
        }
    }
}
