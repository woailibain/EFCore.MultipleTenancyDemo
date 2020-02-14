using kiwiho.Course.MultipleTenancy.EFcore.Api.DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure
{
    public static class MultipleTenancyExtension
    {
        public static IServiceCollection AddConnectionByDatabase(this IServiceCollection services)
        {
            services.AddDbContext<StoreDbContext>((serviceProvider, options)=>
            {
                var resolver = serviceProvider.GetRequiredService<ISqlConnectionResolver>(); 
                
                options.UseMySql(resolver.GetConnection());
            });

            return services;
        }
    }
}