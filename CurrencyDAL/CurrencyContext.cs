using System.IO;
using Microsoft.EntityFrameworkCore;

namespace CurrencyDAL
{
    class CurrencyContext : DbContext
    {

        public DbSet<CurrencyRateRecord> CurrencyRates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder.UseSqlite("Data Source=CurrencyInfo.db");
        }

       
    }
}
