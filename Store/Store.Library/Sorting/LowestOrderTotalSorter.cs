using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Store.Library.Sorting
{
    public class LowestOrderTotalSorter : ISorter
    {
        public IEnumerable<Order> SortOrders(IEnumerable<Order> orders)
        {
            return orders.OrderBy(x => x.OrderTotal);
        }
    }
}
