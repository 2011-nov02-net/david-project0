using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Library
{
    public class Inventory
    {
        // backing field for "ProductId"
        private int _productId;
        // backing field for "Quantity"'
        private int _quantity;

        /// <summary>
        /// The Product Id
        /// </summary>
        public int ProductId
        {
            get { return _productId; }
            private set
            {
                //make sure that it is greater than zero
                if (value > 0)
                    _productId = value;
                else
                    throw new ArgumentOutOfRangeException("ProductId", "The product ID must be a value greater than zero");
            }
        }

        /// <summary>
        /// The quantity in stock at a location
        /// </summary>
        public int Quantity { 
            get { return _quantity; }
            private set
            {
                if (value >= 0)
                    _quantity = value;
                else
                    throw new ArgumentOutOfRangeException("Quantity", "The Quantity must be a value greater or equal to zero");
            }
        }

        /// <summary>
        /// Constructor for the Inventory
        /// Will default to zero inventory
        /// </summary>
        /// <param name="id">Product Id</param>
        public Inventory(int id)
        {
            this.ProductId = id;
            this.Quantity = 0;
        }

        public void AddInventory(int quantity)
        {
            throw new NotImplementedException("Not implemented");
        }
    }
}
