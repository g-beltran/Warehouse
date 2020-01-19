using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Infrastructure.Data
{
    public static class Enumerations
    {
        public enum Roles
        {
            Administrator = 1,
            Standard = 2
        }

        public enum OrderStatus
        {
            Active = 1,
            Canceled = 2
        }
    }
}
