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
            this.ProductObj = new Product("Empty", id, 1.0m, null, quantity + 1);
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

        /// <summary>
        /// AddInventory to the product
        /// </summary>
        /// <remarks>
        /// Since the product is tied to the inventory we don't have to search for it here
        /// The Location will handle all searching and maintaining features
        /// </remarks>
        /// <param name="quantity">The Quantity to add</param>
        public void AddInventory(int quantity)
        {
            //check to make sure that new quantity is greater than zero
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException("Add Quantity", "The quantity to add to Inventory must be a positive value");
            this.Quantity += quantity;
        }

        /// <summary>
        /// SellInventory sells the product
        /// </summary>
        /// <remarks>
        /// Will check to make sure that there is enough quantity to
        /// make the sale, if not, it will throw an
        /// ArgumentOutOfRangeException. Also checks against the order
        /// limit to make sure the product isn't being over sold.
        /// </remarks>
        /// <param name="quantity">The Quantity to Sell</param>
        public void SellInventory(int quantity)
        {
            //make sure quantity <= inventory quantity and that we don't hit the order limit of the product
            if (quantity <= this.Quantity && quantity <= ProductObj.OrderLimit)
                this.Quantity -= quantity;
            else
            {
                if (quantity > this.Quantity)
                    throw new ArgumentOutOfRangeException("Sell Quantity", "Attempt to sell more than currently in stock at location.");
                else
                    throw new ArgumentOutOfRangeException("Sell Quantity", "Attempt to sell more than the order limit of the product.");
            }
        }

        /// <summary>
        /// Get what product it is
        /// </summary>
        /// <remarks>
        /// gets the product information and returns it as an object
        /// </remarks>
        /// <returns>The Product</returns>
        public Product GetProduct()
        {
            return ProductObj;
        }
    }
}
