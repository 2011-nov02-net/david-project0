using System;
using System.Collections.Generic;

#nullable disable

namespace Store.DatabaseModels
{
    public partial class Location
    {
        public Location()
        {
            Customers = new HashSet<Customer>();
            Inventories = new HashSet<Inventory>();
            Orders = new HashSet<Order>();
        }

        public string Name { get; set; }
        public int Id { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
