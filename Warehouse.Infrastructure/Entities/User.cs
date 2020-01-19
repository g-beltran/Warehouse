using System;
using System.Collections.Generic;

namespace Warehouse.Infrastructure.Entities
{
    public partial class User
    {
        public User()
        {
            Order = new HashSet<Order>();
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public int Id { get; set; }
        public short Role { get; set; }

        public virtual ICollection<Order> Order { get; set; }
    }
}
