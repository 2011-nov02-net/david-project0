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
