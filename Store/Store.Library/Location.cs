using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Library
{
    class Location
    {
        // backing field for "Name" field
        private string _name;

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
        public string Id { get; }

        public Location(string name, string id)
        {
            this.Name = name;
            this.Id = id;
        }
    }
}
