using eCommerceSite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options)
            : base(options) // base is like super keyword for Java
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }

    }
}
