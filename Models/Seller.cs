/*
 * Name: Stevenson Suhardy
 * Date: November 21, 2022
 * Student ID: 100839397
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductSellerWebsite.Models
{
    public class Seller
    {
        // Public attributes
        public int SellerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // Creating a list of products for seller (One to Many relationship)
        public List<Product> Products { get; set; }
    }
}
