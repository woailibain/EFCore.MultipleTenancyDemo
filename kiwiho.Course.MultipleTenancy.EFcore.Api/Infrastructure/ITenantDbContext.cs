using System;

namespace kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure
{
    public interface ITenantDbContext
    {
        TenantInfo TenantInfo{get;}
    }
}
