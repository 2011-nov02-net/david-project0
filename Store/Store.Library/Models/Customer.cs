using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Library
{
    public class Customer
    {
        // backing field for "FirstName" property
        private string _firstName;
        // backing field for "LastName" property
        private string _lastName;
        // Backing field for "Id" property
        private int _id;

        /// <summary>
        /// The customer's first name, Must have a value
        /// </summary>
        public string FirstName
        {
            get => _firstName;
            set
            {
                if(value.Length == 0)
                {
                    //checks to make sure that there is a name provided
                    throw new ArgumentException("Cannot have blank First Name");
                }
                _firstName = value;
            }
        }

        /// <summary>
        /// The customer's last name, must have a value
        /// </summary>
        public string LastName
        {
            get => _lastName;
            set
            {
                if(value.Length == 0)
                {
                    throw new ArgumentException("Cannot have blank Last Name");
                }
                _lastName = value;
            }
        }

        public int Id {
            get { return _id; }
            private set
            {
                if (value > 0)
                    this._id = value;
                else
                    throw new ArgumentOutOfRangeException("id", "Id must be positive");
            }
        }

        public Customer(string firstName, string lastName, int id)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Id = id;
        }

        /// <summary>
        /// Override of ToString methdo
        /// </summary>
        /// <remarks>
        /// Just returns the first name concatinated witht he second string to return the name
        /// </remarks>
        /// <returns>Full Name</returns>
        public override string ToString()
        {
            return FirstName + " " + LastName;
        }

    }
}
