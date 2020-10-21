using Microsoft.EntityFrameworkCore;
using biz.dfch.CS.SampleIPA.StockManagement.API.Models;

namespace biz.dfch.CS.SampleIPA.StockManagement.API.Data
{
    public class StockManagementContext : DbContext
    {
        public StockManagementContext(DbContextOptions<StockManagementContext> options)
            : base(options) { }

        public DbSet<Product> Products;
        public DbSet<Booking> Bookings;
        public DbSet<Category> Categories;
    }
}