using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Store.Library
{
    public class LocationRepository
    {
        private readonly ICollection<Location> _location;
        private static int _idCounter;

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
        public LocationRepository()
        {
            _location = new List<Location>();
            _idCounter = 1;
        }

        /// <summary>
        /// Add a location to the list of locations
        /// </summary>
        /// <param name="location">The location to be added</param>
        public void AddLocation(Location location)
        {
            if(_location.Any(l => l.Id == location.Id))
            {
                throw new InvalidOperationException($"Location with ID {location.Id} already exits.");
            }
            _location.Add(location);
        }

        /// <summary>
        /// Creates and returns the location object
        /// </summary>
        /// <remarks>
        /// Make the Location with this method to ensure that the id gets set
        /// sequentially
        /// </remarks>
        /// <param name="name">Name of the location</param>
        /// <returns>The Location</returns>
        public Location CreateLocation(string name)
        {
            Location location = new Location(name, _idCounter);
            _idCounter++;
            return location;
        }

        /// <summary>
        /// Get and return location with a given id
        /// </summary>
        /// <param name="id">Id of the location we want</param>
        /// <returns>The Location</returns>
        public Location GetLocation(int id)
        {
            return _location.First(l => l.Id == id);
        }

        /// <summary>
        /// Get a location with a given name, will only return the first instance
        /// </summary>
        /// <remarks>
        /// This will only return first instance of the location
        /// As the repo only requires unique id values, there could be 
        /// multiple locations with the same name.
        /// Better to use GetLocation(int id) as
        /// that is a unique value
        /// </remarks>
        /// <param name="name"></param>
        /// <returns></returns>
        public Location GetLocation(string name)
        {
            return _location.First(l => l.Name == name);
        }

        public ICollection<Location> GetAllLocations()
        {
            return _location;
        }
    }
}
