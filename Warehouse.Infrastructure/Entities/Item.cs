using System;
using System.Collections.Generic;

namespace Warehouse.Infrastructure.Entities
{
    public partial class Item
    {
        public Item()
        {
            Order = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Sku { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }

        public virtual ICollection<Order> Order { get; set; }
    }
}
