using Microsoft.EntityFrameworkCore;
using biz.dfch.CS.SampleIPA.StockManagement.API.Models;

namespace biz.dfch.CS.SampleIPA.StockManagement.API.Data
{
    public class StockManagementContext : DbContext
    {
        public StockManagementContext(DbContextOptions<StockManagementContext> options)
            : base(options) { }

        public DbSet<Products> Products { get; set; }
        public DbSet<Bookings> Bookings { get; set; }
        public DbSet<Categories> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categories>().HasData(
                new Categories { Id = 1, Name = "Elektronik" },
                new Categories { Id = 2, Name = "Haushalt" },
                new Categories { Id = 3, Name = "Möbel" },
                new Categories { Id = 4, Name = "Kleidung" },
                new Categories { Id = 5, Name = "Sport und Freizeit" },
                new Categories { Id = 6, Name = "Diverses" }
                );
        }
    }
}