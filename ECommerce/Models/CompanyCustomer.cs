using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class CompanyCustomer
    {
        public int CompanyCustomerId { get; set; }
        public int CustomerId { get; set; }
        public int CompanyId { get; set; }

        public virtual Customer Customers { get; set; }
        public virtual Company Companies { get; set; }
    }
}