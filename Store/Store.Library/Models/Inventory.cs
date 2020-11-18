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
        
        // Store an actual product instead of just an id,
        // If we store just an id here we will need a database of
        // products to query and we don't have that yet
        // will leave in productId for future implementations
        // that will have a database to link to.

        /// <summary>
        /// The quantity in stock at a location
        /// </summary>
        public int Quantity 
        { 
            get { return _quantity; }
            private set
            {
                if (value >= 0)
                    _quantity = value;
                else
                    throw new ArgumentOutOfRangeException("Quantity", "The Quantity must be a value greater or equal to zero");
            }
        }

        public Product ProductObj { get; }


        /// <summary>
        /// Constructor for Inventory
        /// With Product object and a Quantity
        /// </summary>
        /// <param name="prod">The Product Object</param>
        /// <param name="quantity">How many in Inventory</param>
        public Inventory(Product prod, int quantity)
        {
            this.ProductObj = prod;
            this.Quantity = quantity;
        }
    }
}
