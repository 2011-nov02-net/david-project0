using System;
using System.Collections.Generic;

#nullable disable

namespace Store.DatabaseModels
{
    public partial class Product
    {
        public Product()
        {
            Inventories = new HashSet<Inventory>();
            Sales = new HashSet<Sale>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int OrderLimit { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
