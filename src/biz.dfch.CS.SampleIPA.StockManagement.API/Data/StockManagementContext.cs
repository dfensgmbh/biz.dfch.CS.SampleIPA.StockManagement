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
    }
}