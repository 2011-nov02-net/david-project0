using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Store.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace Store.Library
{
    public class LocationRepository
    {
        private readonly DbContextOptions<Project0Context> _dbContext;

        /// <summary>
        /// Constructor that set up the connection for the db
        /// </summary>
        /// <param name="contextOptions">The Database Connection</param>
        public LocationRepository(DbContextOptions<Project0Context> contextOptions)
        {
            _dbContext = contextOptions;
        }

        /// <summary>
        /// Add a location to the list of locations
        /// </summary>
        /// <param name="location">The location to be added</param>
        public void AddLocation(string name)
        {
            // get the context of the db
            using var context = new Project0Context(_dbContext);

            if (name.Length > 0)
            {
                // create the db model to add
                DatabaseModels.Location location = new DatabaseModels.Location() { Name = name };

                //add location to context and save
                context.Add(location);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Get and return location with a given id
        /// </summary>
        /// <param name="id">Id of the location we want</param>
        /// <returns>The Location</returns>
        public Location GetLocation(int id)
        {
            // get the context of the db
            using var context = new Project0Context(_dbContext);

            // find the location in the db that has said id
            var dbLocation = context.Locations.FirstOrDefault(l => l.Id == id);

            // check for null value
            if (dbLocation == null) return null;

            return new Location(dbLocation.Name, dbLocation.Id);
        }

        /// <summary>
        /// Gets a new list of all locations
        /// </summary>
        /// <returns>All Locations</returns>
        public ICollection<Location> GetAllLocations()
        {
            // set up context
            using var context = new Project0Context(_dbContext);

            // get all locations from db
            var dbLocations = context.Locations.ToList();

            // convert and return
            return dbLocations.Select(l => new Location(l.Name, l.Id)).ToList();
        }

        /// <summary>
        /// check to see if the id given is an actual location
        /// </summary>
        /// <param name="id">The id we want to check</param>
        /// <returns>True if location exists, False otherwise</returns>
        public bool IsLocation(int id)
        {
            // set up context
            using var context = new Project0Context(_dbContext);
            return context.Locations.Any(l => l.Id == id);
        }

        /// <summary>
        /// check to see if the name given is an actual location
        /// </summary>
        /// <param name="id">The name of the location we want to check</param>
        /// <returns>True if location exists, False otherwise</returns>
        public bool IsLocation(string name)
        {
            // set up context
            using var context = new Project0Context(_dbContext);
            return context.Locations.Any(l => l.Name == name);
        }

        /// <summary>
        /// Gets the number of locations currently in the db
        /// </summary>
        /// <returns>Number of locations</returns>
        public int NumberOfLocations()
        {
            // set up context
            using var context = new Project0Context(_dbContext);

            return context.Locations.ToList().Count;
        }

        /// <summary>
        /// Gets the inventory at the location given
        /// </summary>
        /// <remarks>
        /// The db will return everything in terms of DatabaseModels.Inventory
        /// so this will convert it to our Library.Inventory with a Product object
        /// stored with it
        /// </remarks>
        /// <param name="location">The Location to get inventory from</param>
        /// <returns>The Invetory at location</returns>
        public ICollection<Inventory> GetLocationInventory(Location location)
        {
            // set up context
            using var context = new Project0Context(_dbContext);

            // get the inventory for each location
            var dbInventory = context.Inventories.Where(l => l.LocationId == location.Id).Include(l => l.Product).ToList();

            // get the products related to each
            var inventory = new List<Inventory>();

            foreach(var item in dbInventory)
            {
                // create our converted product
                Product prod = new Product(item.Product.Name, item.Product.Id, item.Product.Price, item.Product.Description, item.Product.OrderLimit);

                // create the new inventory
                inventory.Add(new Inventory(prod, item.Quantity));
            }

            return inventory;
        }

        /// <summary>
        /// Checks to see if a product is at a particular location
        /// </summary>
        /// <param name="location">The location</param>
        /// <param name="productId">the product id</param>
        /// <returns>True if product id is at the location, false other wise</returns>
        public bool IsInLocationInventory(Location location, int productId)
        {
            // set up context
            using var context = new Project0Context(_dbContext);

            //search the inventory with location id and product ID
            return context.Inventories.Any(i => i.LocationId == location.Id && i.ProductId == productId);
        }

        /// <summary>
        /// Adds a given quantity of a product to a locaion
        /// </summary>
        /// <param name="location">The location to add to</param>
        /// <param name="productId">The product to add</param>
        /// <param name="quantity">The amount to add</param>
        /// <returns>True if sucessful, false otherwise</returns>
        public bool AddLocationInventory(Location location, int productId, int quantity)
        {
            // set up context
            using var context = new Project0Context(_dbContext);
            // get first match in inventory that matches the location id and the product id
            var inventory = context.Inventories.First(i => i.LocationId == location.Id && i.ProductId == productId);

            //nested in a try catch block so we can report an error to the user
            try
            {
                // attempt to add inventory
                inventory.Quantity += quantity;
                context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// Adds a given quantity of a product to a locaion
        /// </summary>
        /// <param name="location">The location to add too</param>
        /// <param name="product">The product to add</param>
        /// <param name="quantity">amount to add</param>
        /// <returns></returns>
        public bool AddLocationInventory(Location location, DatabaseModels.Product product, int quantity)
        {
            // set up context
            using var context = new Project0Context(_dbContext);

            // make the new inventory
            var inventory = new DatabaseModels.Inventory
            {
                LocationId = location.Id,
                Quantity = quantity,
                ProductId = product.Id
            };

            context.Inventories.Add(inventory);

            // ensure that the save works successfully
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Removes a whole product from inventory
        /// </summary>
        /// <remarks>
        /// Will remove the entire product, currently there is no way to remove a particular quantity yet
        /// </remarks>
        /// <param name="location">The location to remove from</param>
        /// <param name="productId">The product to remove</param>
        /// <returns>true is sucessful, false otherwise</returns>
        public bool RemoveLocationInventory(Location location, int productId)
        {
            // set up context
            using var context = new Project0Context(_dbContext);

            // nest in a try catch block to report error to user
            try
            {
                context.Remove(context.Inventories.First(i => i.LocationId == location.Id && i.ProductId == productId));
                context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks to see if there is enough inventory of a product to complete an order
        /// </summary>
        /// <param name="location">Location that the order is being placed at</param>
        /// <param name="productId">Product that is being ordered</param>
        /// <param name="quantity">amount of said product</param>
        /// <returns>true if enough, false otherwise</returns>
        public bool IsEnoughInventory(Location location, int productId, int quantity)
        {
            // set up context
            using var context = new Project0Context(_dbContext);

            return quantity <= context.Inventories.First(i => i.LocationId == location.Id && i.ProductId == productId).Quantity;
        }
    }
}
