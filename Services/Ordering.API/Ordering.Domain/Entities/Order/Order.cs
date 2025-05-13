using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ordering.Domain.Common;
using Ordering.Domain.Entities.Payment;

namespace Ordering.Domain.Entities.Order
{
    public class Order : EntityBase
    {
        public string UserName { get; set; }

        public ICollection<Address.Address> Addresses { get; set; }

        public decimal TotalPrice { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string CardName { get; set; }
        public string RefCode { get; set; }
        PaymentMethod PaymentMethod { get; set; }


    }

}

