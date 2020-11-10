using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Store.Library
{
    public class Order
    {
        public int CustomerId { get; set; }
        public int LocationId { get; set; }
        public ICollection<Sale> SalesList { get; set; }
        public DateTime Date { get; set; }

        public decimal GetTotalOfOrder()
        {
            return SalesList.Sum(sale => sale.ProductObj.Price * sale.SaleQuantity);
        }

        public Order(int custId, int locId, List<Sale> sales, DateTime date)
        {
            this.CustomerId = custId;
            this.LocationId = locId;
            this.SalesList = sales;
            this.Date = date;
        }
    }
}
