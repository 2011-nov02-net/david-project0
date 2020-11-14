using System;
using System.Collections.Generic;

#nullable disable

namespace Store.DatabaseModels
{
    public partial class Order
    {
        public Order()
        {
            Sales = new HashSet<Sale>();
        }

        public int CustomerId { get; set; }
        public int LocationId { get; set; }
        public DateTime Date { get; set; }
        public int OrderNumber { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Location Location { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
