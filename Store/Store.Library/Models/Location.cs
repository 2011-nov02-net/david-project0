using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Store.Library
{
    public class Location
    {
        // backing field for "Name" field
        private string _name;
        // backing field for "Id" field
        private int _id;

        /// <summary>
        /// The Name of the store, must have a value
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if(value.Length == 0)
                {
                    throw new ArgumentException("The Location of the store must have a name.");
                }
                _name = value;
            }
        }

        /// <summary>
        /// The ID of the store.
        /// </summary>
        /// <remarks>
        /// Just the get method for the _id value since the Location Repository will handle the creation of the id value
        /// </remarks>
        public int Id {
            get { return _id; }
            private set
            {
                if (value > 0)
                    _id = value;
                else
                    throw new ArgumentOutOfRangeException("id", "Id must be positive");
            }
        }

        public ICollection<Inventory> LocationInventory { get; }

        public Location(string name, int id)
        {
            this.Name = name;
            this.Id = id;
            this.LocationInventory = new List<Inventory>();
        }

        public Location() 
        {
            this.LocationInventory = new List<Inventory>();
        }

        /// <summary>
        /// Adds inventory to the location
        /// </summary>
        /// <remarks>
        /// Will check to see if product exitst before adding. If product does exist it will get the inventory
        /// of that product and add directly to it. Both methods have to be within try catch blocks as the 
        /// .AddInventory will throw an error if the quantity is less than 1.  Returns true if Inventory
        /// was added, false if inventory was not added.
        /// </remarks>
        /// <param name="prod">Product Want to Add</param>
        /// <param name="quantity">Amount of Product Quantity</param>
        /// <returns>True if Successful, False otherwise</returns>
        public bool AddItemToInventory(Product prod, int quantity)
        {
            // check to see if product already exits
            if(CheckInventoryForProduct(prod))
            {
                // product already exists, add to existing inventory
                try
                {
                    (LocationInventory.First(p => p.ProductObj.Id == prod.Id)).AddInventory(quantity);
                }
                catch (ArgumentOutOfRangeException)
                {
                    return false;
                }
            }
            else
            {
                // Product does not already exist in the inventory
                // add it
                try
                {
                    LocationInventory.Add(new Inventory(prod, quantity));
                }
                catch (ArgumentOutOfRangeException)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Sell a certain Quantity from Inventory
        /// </summary>
        /// <remarks>
        /// Will check to see if the product exists, if it does will try
        /// to sell the inventory from it.
        /// Will return true if SellInventory was successful,
        /// will return false if not.
        /// </remarks>
        /// <param name="prod">The Product</param>
        /// <param name="quantity">Quantity to sell</param>
        /// <returns>True if Successful, False otherwise</returns>
        public bool SellItemFromInventory(Product prod, int quantity)
        {
            //get and check product
            if(CheckInventoryForProduct(prod))
            {
                try
                {
                    LocationInventory.First(p => p.ProductObj.Id == prod.Id).SellInventory(quantity);
                }
                catch (ArgumentOutOfRangeException)
                {
                    return false;
                }
            }
            else
            {
                // Inventory does not exist for this product, return false
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check to see if the product exists
        /// </summary>
        /// <param name="prod">The Product</param>
        /// <returns>True if exists, False otherwise</returns>
        public bool CheckInventoryForProduct(Product prod)
        {
            return LocationInventory.Any(p => p.ProductObj.Id == prod.Id);
        }

        /// <summary>
        /// Override of ToString method
        /// </summary>
        /// <remarks>
        /// Just returns the name of the store
        /// </remarks>
        /// <returns>Name of store</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
