using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Library
{
    public class Sale
    {
        private int _saleQuantity;
        public int ProductId { get; set; }
        public string ProductName { get; }
        public decimal PurchasePrice { get; }
        public int SaleQuantity
        {
            get { return _saleQuantity; }
            set 
            {
                if (value > 0)
                    _saleQuantity = value;
                else
                    throw new ArgumentOutOfRangeException("SaleQuantity", "Sale Quantity must be greater than zero");
           }
        }

        public Sale(int productId, int quantity)
        {
            this.ProductId = productId;
            this.SaleQuantity = quantity;
        }

        public Sale(int id, string name, decimal price, int quantity)
        {
            this.ProductId = id;
            this.ProductName = name;
            this.PurchasePrice = price;
            this.SaleQuantity = quantity;
        }
    }
}
