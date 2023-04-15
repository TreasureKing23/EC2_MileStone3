using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NCB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NCB.Repositories.Data
{
    public class ApiDbContext : IdentityDbContext<ApplicationUser>
    {


        public ApiDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Seed Database
            builder.Entity<Currency>().HasData(
                new Currency { Id = 1, Name = "Jamaican Dollar", ShortName = "JMD" },
                new Currency { Id = 2, Name = "United States Dollar", ShortName = "USD" },
                new Currency { Id = 3, Name = "European Dollar", ShortName = "EURO" }

             );
        }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Transaction> Transactions { get; set; }
    }
}
