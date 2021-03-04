using Listings.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Listings.Web.Models
{
    public class ListingsViewModel
    {
        public List<Listing> Listings { get; set; }
        public List<int> Ids { get; set; }
    }
}
