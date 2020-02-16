using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace kiwiho.Course.MultipleTenancy.EFcore.Api.DAL
{
    public class StoreDbContext : DbContext,ITenantDbContext
    {
        public DbSet<Product> Products => this.Set<Product>();

        public TenantInfo TenantInfo => tenantInfo;

        private readonly TenantInfo tenantInfo;

        public StoreDbContext(DbContextOptions options, TenantInfo tenantInfo) : base(options)
        {
            this.tenantInfo = tenantInfo;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable(this.tenantInfo.Name + "_Products");
        }
    }

    

}