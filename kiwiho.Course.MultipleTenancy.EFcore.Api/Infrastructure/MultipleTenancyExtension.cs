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
                string key = "default", DatabaseIntegration database = DatabaseIntegration.Mysql)
            where TDbContext : DbContext, ITenantDbContext
        {
            var option = new ConnectionResolverOption()
            {
                Key = key,
                Type = ConnectionResolverType.ByDatabase,
                DBType = database
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
                    DBType = DatabaseIntegration.Mysql
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
                var dbContextManager = serviceProvider.GetService<IDbContextManager>();
                var resolver = serviceProvider.GetRequiredService<ISqlConnectionResolver>();

                DbContextOptionsBuilder dbOptionBuilder = null;
                switch (option.DBType)
                {
                    case DatabaseIntegration.SqlServer:
                        dbOptionBuilder = options.UseSqlServer(resolver.GetConnection());
                        break;
                    case DatabaseIntegration.Mysql:
                        dbOptionBuilder = options.UseMySql(resolver.GetConnection());
                        break;
                    default:
                        throw new System.NotSupportedException("db type not supported");
                }
                if (option.Type == ConnectionResolverType.ByTabel || option.Type == ConnectionResolverType.BySchema)
                {
                    dbOptionBuilder.ReplaceService<IModelCacheKeyFactory, TenantModelCacheKeyFactory<TDbContext>>();
                }
            });

            return services;
        }

        public static IServiceCollection AddTenantDatabasePerTable<TDbContext>(this IServiceCollection services,
                string connectionStringName, string key = "default", DatabaseIntegration database = DatabaseIntegration.Mysql)
            where TDbContext : DbContext, ITenantDbContext
        {
            var option = new ConnectionResolverOption()
            {
                Key = key,
                Type = ConnectionResolverType.ByTabel,
                ConnectinStringName = connectionStringName,
                DBType = database
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
                    ConnectinStringName = "default",
                    DBType = DatabaseIntegration.Mysql
                };
            }


            return services.AddDatabase<TDbContext>(option);
        }

        public static IServiceCollection AddTenantDatabasePerSchema<TDbContext>(this IServiceCollection services,
                string connectionStringName, string key = "default")
            where TDbContext : DbContext, ITenantDbContext
        {
            var option = new ConnectionResolverOption()
            {
                Key = key,
                Type = ConnectionResolverType.BySchema,
                ConnectinStringName = connectionStringName,
                DBType = DatabaseIntegration.SqlServer
            };


            return services.AddTenantDatabasePerSchema<TDbContext>(option);
        }

        public static IServiceCollection AddTenantDatabasePerSchema<TDbContext>(this IServiceCollection services,
                ConnectionResolverOption option)
            where TDbContext : DbContext, ITenantDbContext
        {
            if (option == null)
            {
                option = new ConnectionResolverOption()
                {
                    Key = "default",
                    Type = ConnectionResolverType.BySchema,
                    ConnectinStringName = "default",
                    DBType = DatabaseIntegration.SqlServer
                };
            }

            return services.AddTenantDatabasePerTable<TDbContext>(option);
        }
    }
}