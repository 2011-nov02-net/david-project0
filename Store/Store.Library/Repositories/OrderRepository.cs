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
        private readonly ICollection<Order> _orders;
        private static int _orderIdCounter;

        private readonly DbContextOptions<Project0Context> _dbContext;

        public OrderRepository(ICollection<Order> orders)
        {
            _orders = orders ?? throw new ArgumentNullException(nameof(orders));
            _orderIdCounter = orders.Count + 1;
        }

        public OrderRepository(DbContextOptions<Project0Context> contextOptions)
        {
            _orders = new List<Order>();
            _orderIdCounter = 1;
            _dbContext = contextOptions;
        }

        public void AddOrder(Customer customer, Location location, ICollection<Sale> sales)
        {
            // get the context of the db
            using var context = new Project0Context(_dbContext);

            // create list converting from Library.Sale to DatabaseModel.Sale

            var dbSales = new List<DatabaseModels.Sale>();

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

            context.Orders.Add(order);
            context.SaveChanges();
        }

        public List<Order> GetAllOrders()
        {
            // get the context of the db
            using var context = new Project0Context(_dbContext);

            var dbOrders = context.Orders.ToList();

            return dbOrders.Select(o => new Order(o.CustomerId, o.LocationId, o.Date, o.OrderNumber)).ToList();
        }

        public List<Order> GetAllOrdersByCustomer(int custId)
        {
            // get the context of the db
            using var context = new Project0Context(_dbContext);

            //get all orders where the custid of the order is the same as the one we are looking for
            var dbCustOrders = context.Orders.Where(o => o.CustomerId == custId).ToList();
            return dbCustOrders.Select(o => new Order(o.CustomerId, o.LocationId, o.Date, o.OrderNumber)).ToList();
        }

        public List<Order> GetAllOrdersByLocation(int locId)
        {
            // get the context of the db
            using var context = new Project0Context(_dbContext);

            //get all orders where the locationid of the order is the same as the one we are looking for
            var dbLocationOrders = context.Orders.Where(o => o.LocationId == locId).ToList();
            return dbLocationOrders.Select(o => new Order(o.CustomerId, o.LocationId, o.Date, o.OrderNumber)).ToList();
        }

        public bool IsPrevOrder(Order order)
        {
            return _orders.Any(o => o.OrderNumber == order.OrderNumber);
        }

        public decimal GetOrderTotal(int orderId)
        {
            // get the context of the db
            using var context = new Project0Context(_dbContext);

            var list = context.Sales.Where(s => s.OrderNumber == orderId).ToList();

            decimal sum = 0;

            foreach(var item in list)
            {
                sum += item.PurchasePrice;
            }

            return sum;
        }
    }
}
