using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Library
{
    public class Sale
    {
        private int _saleQuantity;
        public int ProductId { get; set; }
        public Product ProductObj { get; set; }
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

        public Sale(Product prod, int quantity)
        {
            this.ProductObj = prod;
            this.SaleQuantity = quantity;
        }

        public Sale(int productId, int quantity)
        {
            this.ProductId = productId;
            this.SaleQuantity = quantity;
        }
    }
}
