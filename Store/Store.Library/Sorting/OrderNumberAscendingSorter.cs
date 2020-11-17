using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Store.Library.Sorting
{
    public class OrderNumberAscendingSorter : ISorter
    {
        public IEnumerable<Order> SortOrders(IEnumerable<Order> orders)
        {
            return orders.OrderBy(o => o.OrderNumber);
        }
    }
}