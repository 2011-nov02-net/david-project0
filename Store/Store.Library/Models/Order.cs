using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Store.Library
{
    public class Order
    {
        private int _orderNumber;
        public int CustomerId { get; set; }
        public int LocationId { get; set; }
        public ICollection<Sale> SalesList { get; set; }
        public DateTime Date { get; set; }
        public int OrderNumber
        {
            get { return _orderNumber; }
            private set
            {
                if (value > 0)
                    this._orderNumber = value;
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

        public Order(int custId, int locId, DateTime date, int orderNumber)
        {
            this.CustomerId = custId;
            this.LocationId = locId;
            this.Date = date;
            this.OrderNumber = orderNumber;
        }
    }
}
