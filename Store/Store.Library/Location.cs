using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Store.Library
{
    public class Location
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
        public int Id { get; }

        public Location(string name, int id)
        {
            this.Name = name;
            // just simple error checking here
            // make sure id is positive, but we rely on the repo to ensure that ids are handed out in order
            if (id > 0)
                this.Id = id;
            else
                throw new ArgumentOutOfRangeException("id", "Id must be positive");


        }

        public Location() { }
    }
}
