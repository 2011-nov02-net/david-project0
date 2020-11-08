﻿using System;
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
            get { return Id; }
            private set
            {
                if (value > 0)
                    this.Id = value;
                else
                    throw new ArgumentOutOfRangeException("id", "Id must be positive");
            }
        }

    }
}
