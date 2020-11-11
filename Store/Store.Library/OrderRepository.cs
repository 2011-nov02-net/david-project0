using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Store.Library
{
    public class OrderRepository
    {
        private readonly ICollection<Order> _orders;
        private static int _orderIdCounter;

        public OrderRepository(ICollection<Order> orders)
        {
            _orders = orders ?? throw new ArgumentNullException(nameof(orders));
            _orderIdCounter = orders.Count + 1;
        }

        public OrderRepository()
        {
            _orders = new List<Order>();
            _orderIdCounter = 1;
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
            return new List<Order>(_orders);
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
    }
}
