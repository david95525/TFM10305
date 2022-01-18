using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TeaBar.Models;

namespace TeaBar.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Products> Products { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Stores> Stores { get; set; }
        public DbSet<Discount> Discount { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<News> News { get; set; }
        public virtual DbSet<ApplicationUsers> ApplicationUsers { get; set; }
        public virtual DbSet<UserOfCustomer> UserOfCustomer { get; set; }
    }
}
