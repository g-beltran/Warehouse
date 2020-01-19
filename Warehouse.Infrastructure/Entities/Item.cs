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

        public string Sku { get; set; }
        public string Description { get; set; }
        public int StockQuantity { get; set; }

        public virtual ICollection<Order> Order { get; set; }
    }
}
