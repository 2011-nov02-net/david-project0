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
        private readonly ICollection<Location> _location;
        private static int _idCounter;

        private readonly DbContextOptions<Project0Context> _dbContext;

        /// <summary>
        /// Constructor that will take a preformed set of location and store it
        /// </summary>
        /// <param name="location"> the Collection of location</param>
        public LocationRepository(ICollection<Location> location)
        {
            _location = location ?? throw new ArgumentNullException(nameof(location));
            //set the id counter
            _idCounter = location.Count + 1;
        }

        /// <summary>
        /// Constructor that will make an empty list of location
        /// </summary>
        public LocationRepository(DbContextOptions<Project0Context> contextOptions)
        {
            _location = new List<Location>();
            _idCounter = 1;
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

        public int NumberOfLocations()
        {
            // set up context
            using var context = new Project0Context(_dbContext);

            return context.Locations.ToList().Count;
        }
    }
}
