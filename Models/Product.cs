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
    public class Product
    {
        // Public attributes
        public int ProductID { get; set; }

        public string Name { get; set; }

        public float PricePerUnit { get; set; }

        public int Year { get; set; }

        // Creating a SellerID foreign key to link with the Seller table
        public int SellerID { get; set; }
        public Seller Seller { get; set; }
    }
}
