using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Store.Library.Sorting
{
    public class OrderNumberDescendingSorter : ISorter
    {
        public IEnumerable<Order> SortOrders(IEnumerable<Order> orders)
        {
            return orders.OrderByDescending(o => o.OrderNumber);
        }
    }
}
