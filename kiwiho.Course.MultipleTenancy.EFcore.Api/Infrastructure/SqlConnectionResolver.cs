using System;
using Microsoft.Extensions.Configuration;

namespace kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure
{
    public interface ISqlConnectionResolver
    {
        string GetConnection();

    }

    public class TenantSqlConnectionResolver : ISqlConnectionResolver
    {
        private readonly TenantInfo tenantInfo;
        private readonly IConfiguration configuration;
        private readonly ConnectionResolverOption option;

        public TenantSqlConnectionResolver(TenantInfo tenantInfo, IConfiguration configuration, ConnectionResolverOption option)
        {
            this.option = option;
            this.tenantInfo = tenantInfo;
            this.configuration = configuration;
        }
        public string GetConnection()
        {
            string connectionString = null;
            switch (this.option.Type)
            {
                case ConnectionResolverType.ByDatabase:
                    connectionString = configuration.GetConnectionString(this.tenantInfo.Name);
                    break;
                case ConnectionResolverType.ByTabel:
                case ConnectionResolverType.BySchema:
                    connectionString = configuration.GetConnectionString(this.option.ConnectinStringName);
                    break;
            }
             
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new NullReferenceException("can not find the connection");
            }
            return connectionString;
        }
    }
}