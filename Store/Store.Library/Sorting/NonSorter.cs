using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Store.Library.Sorting
{
    public class NonSorter : ISorter
    {
        public IEnumerable<Order> SortOrders(IEnumerable<Order> orders)
        {
            return orders.ToList();
        }
    }
}
