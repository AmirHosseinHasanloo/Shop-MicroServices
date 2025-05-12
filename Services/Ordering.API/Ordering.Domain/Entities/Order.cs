using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ordering.Domain.Common;

namespace Ordering.Domain.Entities
{
    public class Order : EntityBase
    {
        public string UserName { get; set; }

        //TODO : Add Address class

        //TODO : Add ContanctInfo class

        public decimal TotalPrice { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        // payment method
        public string CardName { get; set; }
        public string RefCode { get; set; }
        public int PaymentMethod { get; set; }

    }

}

