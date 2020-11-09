using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Library
{
    public class Product
    {
        // backing field for "Name" field
        private string _name;
        // backing field for "Id" field
        private int _id;
        // backing field for "Price" field
        private decimal _price;

        /// <summary>
        /// The Name of the Product, must have a value
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (value.Length == 0)
                {
                    throw new ArgumentException("The Product must have a name.");
                }
                _name = value;
            }
        }

        /// <summary>
        /// The ID of the Product.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public int Id
        {
            get { return _id; }
            private set
            {
                if (value > 0)
                    _id = value;
                else
                    throw new ArgumentOutOfRangeException("id", "Id must be positive");
            }
        }

        /// <summary>
        /// The price of the product
        /// </summary>
        public decimal Price {
            get { return _price; }
            set
            {
                // check to make sure it is positive
                if (value > 0)
                    this._price = value;
                else
                    throw new ArgumentOutOfRangeException("Price", "Price must be greater than 0.");
            }
        }

        /// <summary>
        /// The Description of the product
        /// </summary>
        public string Description { get; set; }

        public Product(string name, int id, decimal price, string description)
        {
            this.Name = name;
            this.Id = id;
            this.Price = price;
            this.Description = description;
        }
    }
}
