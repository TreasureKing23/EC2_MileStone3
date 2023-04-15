using _1903966_Milestone2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1903966_Milestone2.Repositories.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Shoe> Shoes { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<ShoeBrand> ShoeBrands { get; set; }
        public DbSet<Order> Orders  { get; set; }
    }
}
