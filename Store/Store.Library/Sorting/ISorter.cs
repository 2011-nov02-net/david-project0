using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Library
{
    public interface ISorter
    {
        IEnumerable<Order> SortOrders(IEnumerable<Order> orders);
    }
}
