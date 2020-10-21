using Microsoft.EntityFrameworkCore;
using biz.dfch.CS.SampleIPA.StockManagement.API.Models;

namespace biz.dfch.CS.SampleIPA.StockManagement.API.Data
{
    public class StockManagementContext : DbContext
    {
        public StockManagementContext(DbContextOptions<StockManagementContext> options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Elektronik" },
                new Category { Id = 2, Name = "Haushalt" },
                new Category { Id = 3, Name = "Möbel" },
                new Category { Id = 4, Name = "Kleidung" },
                new Category { Id = 5, Name = "Sport und Freizeit" },
                new Category { Id = 6, Name = "Diverses" }
                );
        }
    }
}