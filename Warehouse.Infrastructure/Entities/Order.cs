using System;
using System.Collections.Generic;

namespace Warehouse.Infrastructure.Entities
{
    public partial class Order
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public DateTime Modified { get; set; }
        public int UserId { get; set; }
        public short Status { get; set; }
        public string Sku { get; set; }

        public virtual Item SkuNavigation { get; set; }
        public virtual User User { get; set; }
    }
}
