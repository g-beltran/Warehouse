using System;
using System.Collections.Generic;

namespace Warehouse.Infrastructure.Entities
{
    public partial class LogOrder
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Sku { get; set; }
        public int Quantity { get; set; }
        public DateTime Modified { get; set; }
        public int UserId { get; set; }
        public short Status { get; set; }
    }
}
