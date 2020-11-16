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

        public void AddOrder(int custId, int locId, List<Sale> sales, DateTime date)
        {
            //create the new order
            Order order = new Order(custId, locId, sales, date, _orderIdCounter);
            _orderIdCounter++;
            _orders.Add(order);
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
            List<Order> custOrders = _orders.Where(o => o.CustomerId == custId).ToList();
            return custOrders;
        }

        public List<Order> GetAllOrdersByLocation(int locId)
        {
            List<Order> locOrders = _orders.Where(o => o.LocationId == locId).ToList();
            return locOrders;
        }

        public List<Order> GetAllOrdersByCustomerAndLocation(int custId, int locId)
        {
            List<Order> custAndLocOrders = _orders.Where(o => o.CustomerId == custId && o.LocationId == locId).ToList();
            return custAndLocOrders;
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
