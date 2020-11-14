using System;
using System.Collections.Generic;

#nullable disable

namespace Store.DatabaseModels
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? DefaultLocationId { get; set; }
        public int Id { get; set; }
        public DateTime? BirthDate { get; set; }

        public virtual Location DefaultLocation { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
