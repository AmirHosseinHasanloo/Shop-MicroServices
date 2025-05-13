using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ordering.Domain.Common;
using Ordering.Domain.Entities.Order;

namespace Ordering.Domain.Entities.Address
{
    public class Address : EntityBase
    {
        public string UserAddress { get; set; }
        public int OrderId { get; set; }
        Order.Order order { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
    }
}
