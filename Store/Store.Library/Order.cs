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
        public int OrderNumber
        {
            get { return OrderNumber; }
            private set
            {
                if (value > 0)
                    this.OrderNumber = value;
                else
                    throw new ArgumentOutOfRangeException("OrderNumber", "OrderNumber must be positive");
            }
        }

        public decimal GetTotalOfOrder()
        {
            return SalesList.Sum(sale => sale.ProductObj.Price * sale.SaleQuantity);
        }

        public Order(int custId, int locId, List<Sale> sales, DateTime date, int orderNumber)
        {
            this.CustomerId = custId;
            this.LocationId = locId;
            this.SalesList = sales;
            this.Date = date;
            this.OrderNumber = orderNumber;
        }
    }
}
