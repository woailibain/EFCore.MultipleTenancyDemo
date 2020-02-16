using kiwiho.Course.MultipleTenancy.EFcore.Api.DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure
{
    public static class MultipleTenancyExtension
    {
        public static IServiceCollection AddDatabasePerConnection<TDbContext>(this IServiceCollection services,
                string key = "default")
            where TDbContext : DbContext, ITenantDbContext
        {
            var option = new ConnectionResolverOption()
            {
                Key = key,
                Type = ConnectionResolverType.ByDatabase,
            };
            return services.AddDatabasePerConnection<TDbContext>(option);
        }

        public static IServiceCollection AddDatabasePerConnection<TDbContext>(this IServiceCollection services,
                ConnectionResolverOption option)
            where TDbContext : DbContext, ITenantDbContext
        {
            if (option == null)
            {
                option = new ConnectionResolverOption()
                {
                    Key = "default",
                    Type = ConnectionResolverType.ByDatabase,
                };
            }

            return services.AddDatabase<TDbContext>(option);
        }

        internal static IServiceCollection AddDatabase<TDbContext>(this IServiceCollection services,
                ConnectionResolverOption option)
            where TDbContext : DbContext, ITenantDbContext
        {
            services.AddSingleton(option);

            services.AddScoped<TenantInfo>();
            services.AddScoped<ISqlConnectionResolver, TenantSqlConnectionResolver>();
            services.AddDbContext<TDbContext>((serviceProvider, options) =>
            {
                var tenantInfo = serviceProvider.GetService<TenantInfo>();
                var resolver = serviceProvider.GetRequiredService<ISqlConnectionResolver>();

                var dbOptionBuilder = options.UseMySql(resolver.GetConnection(), builder =>
                {
                    if (option.Type == ConnectionResolverType.ByTabel)
                    {
                        builder.MigrationsHistoryTable($"{tenantInfo.Name}__EFMigrationsHistory");
                    }
                });
                if (option.Type == ConnectionResolverType.ByTabel)
                {
                    dbOptionBuilder.ReplaceService<IModelCacheKeyFactory, TenantModelCacheKeyFactory<TDbContext>>();
                    dbOptionBuilder.ReplaceService<Microsoft.EntityFrameworkCore.Migrations.IMigrationsAssembly, MigrationByTenantAssembly>();
                }
            });

            return services;
        }

        public static IServiceCollection AddTenantDatabasePerTable<TDbContext>(this IServiceCollection services,
                string connectionStringName, string key = "default")
            where TDbContext : DbContext, ITenantDbContext
        {
            var option = new ConnectionResolverOption()
            {
                Key = key,
                Type = ConnectionResolverType.ByTabel,
                ConnectinStringName = connectionStringName
            };

            return services.AddTenantDatabasePerTable<TDbContext>(option);
        }

        public static IServiceCollection AddTenantDatabasePerTable<TDbContext>(this IServiceCollection services,
                ConnectionResolverOption option)
            where TDbContext : DbContext, ITenantDbContext
        {
            if (option == null)
            {
                option = new ConnectionResolverOption()
                {
                    Key = "default",
                    Type = ConnectionResolverType.ByTabel,
                    ConnectinStringName = "default"
                };
            }


            return services.AddDatabase<TDbContext>(option);
        }
    }
}