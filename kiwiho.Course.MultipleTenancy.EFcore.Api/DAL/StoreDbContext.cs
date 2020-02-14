using Microsoft.EntityFrameworkCore;

namespace kiwiho.Course.MultipleTenancy.EFcore.Api.DAL
{
    public class StoreDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public StoreDbContext(DbContextOptions options) : base(options)
        {
        }
    }

}