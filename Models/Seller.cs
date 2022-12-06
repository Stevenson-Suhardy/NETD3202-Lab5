using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductSellerWebsite.Models
{
    public class Seller
    {
        public int SellerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<Product> Products { get; set; }
    }
}
