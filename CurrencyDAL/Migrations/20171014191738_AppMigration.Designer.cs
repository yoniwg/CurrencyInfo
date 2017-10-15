using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CurrencyDAL;

namespace CurrencyDAL.Migrations
{
    [DbContext(typeof(CurrencyContext))]
    [Migration("20171014191738_AppMigration")]
    partial class AppMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.3");

            modelBuilder.Entity("CurrencyDAL.CurrencyRate", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CurrencyCode");

                    b.Property<DateTime>("Date");

                    b.Property<decimal>("Rate");

                    b.HasKey("Id");

                    b.ToTable("CurrencyRates");
                });
        }
    }
}
