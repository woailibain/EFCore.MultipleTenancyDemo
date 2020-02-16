using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure
{
    internal sealed class TenantModelCacheKeyFactory<TContext> : ModelCacheKeyFactory
        where TContext : DbContext, ITenantDbContext
    {

        public override object Create(DbContext context)
        {
            var dbContext = context as TContext;
            return new TenantModelCacheKey<TContext>(dbContext, dbContext?.TenantInfo?.Name ?? "no_tenant_identifier");
        }

        public TenantModelCacheKeyFactory(ModelCacheKeyFactoryDependencies dependencies) : base(dependencies)
        {
        }
    }

    internal sealed class TenantModelCacheKey<TContext> : ModelCacheKey
        where TContext : DbContext, ITenantDbContext
    {
        private readonly TContext context;
        private readonly string identifier;
        public TenantModelCacheKey(TContext context, string identifier) : base(context)
        {
            this.context = context;
            this.identifier = identifier;
        }

        protected override bool Equals(ModelCacheKey other)
        {
            return base.Equals(other) && (other as TenantModelCacheKey<TContext>)?.identifier == identifier;
        }

        public override int GetHashCode()
        {
            var hashCode = base.GetHashCode();
            if (identifier != null)
            {
                hashCode ^= identifier.GetHashCode();
            }

            return hashCode;
        }
    }
}
