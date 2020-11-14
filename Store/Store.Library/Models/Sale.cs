using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Library
{
    public class Sale
    {
        public Product ProductObj { get; set; }
        public int SaleQuantity
        {
            get { return SaleQuantity; }
            set 
            {
                if (value > 0)
                    SaleQuantity = value;
                else
                    throw new ArgumentOutOfRangeException("SaleQuantity", "Sale Quantity must be greater than zero");
           }
        }

        public Sale(Product prod, int quantity)
        {
            this.ProductObj = prod;
            this.SaleQuantity = quantity;
        }
    }
}
