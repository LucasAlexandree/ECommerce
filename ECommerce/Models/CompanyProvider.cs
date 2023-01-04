using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class CompanyProvider
    {
        public int CompanyProviderId { get; set; }
        public int ProviderId { get; set; }
        public int CompanyId { get; set; }

        public virtual Provider Providers { get; set; }
        public virtual Company Companies { get; set; }
    }
}