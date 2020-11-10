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
        // backing field for "Product"
        // private Product _productObj;


        // Store an actual product instead of just an id,
        // If we store just an id here we will need a database of
        // products to query and we don't have that yet
        // will leave in productId for future implementations
        // that will have a database to link to.

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

        public Product ProductObj { get; set; }

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

        /// <summary>
        /// Constructor for the inventory
        /// With only a product ID
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <param name="quantity">Initial Quantity</param>
        public Inventory(int id, int quantity)
        {
            this.ProductId = id;
            this.Quantity = quantity;
        }

        /// <summary>
        /// Constructor for the Inventory
        /// With a Product object being passed in
        /// </summary>
        /// <remarks>
        /// Since this constructor doesn't take in any arguments in regards to quantity,
        /// it will assume that there is no quantity and set it to zero
        /// </remarks>
        /// <param name="prod">The Product Object</param>
        public Inventory(Product prod)
        {
            this.ProductObj = prod;
            this.Quantity = 0;
        }

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

        public void AddInventory(int quantity)
        {
            //check to make sure that new quantity is greater than zero
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException("Add Quantity", "The quantity to add to Inventory must be a positive value");
            this.Quantity += quantity;
        }

        public void SellInventory(int quantity)
        {
            //make sure quantity <= inventory quantity
            if (quantity <= this.Quantity)
                this.Quantity -= quantity;
            else
                throw new ArgumentOutOfRangeException("Sell Quantity", "Attempt to sell more than currently in stock at location");
        }

        public Product GetProduct()
        {
            throw new NotImplementedException("Not implemented");
        }
    }
}
