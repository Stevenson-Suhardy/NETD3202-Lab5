using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductSellerWebsite.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        public string Name { get; set; }

        public float PricePerUnit { get; set; }

        public int Year { get; set; }

        public int SellerID { get; set; }
        public Seller Seller { get; set; }
    }
}
