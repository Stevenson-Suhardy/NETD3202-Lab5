using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ProductSellerWebsite.Models;

namespace ProductSellerWebsite.Data
{
    public class LoginContext : IdentityDbContext<AppUser>
    {
        public LoginContext(DbContextOptions<LoginContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize
        }
    }
}
