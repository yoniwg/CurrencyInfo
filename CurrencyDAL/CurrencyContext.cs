using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CurrencyDAL
{
    public class CurrencyContext : DbContext
    {

        public DbSet<CurrencyRate> CurrencyRates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=CurrencyInfo.db");
        }

       
    }
}
