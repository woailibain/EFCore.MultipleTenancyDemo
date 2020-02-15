using System;
using Microsoft.Extensions.Configuration;

namespace kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure
{
    public interface ISqlConnectionResolver
    {
        string GetConnection();

    }

    public class HttpHeaderSqlConnectionResolver : ISqlConnectionResolver
    {
        private readonly TenantInfo tenantInfo;
        private readonly IConfiguration configuration;

        public HttpHeaderSqlConnectionResolver(TenantInfo tenantInfo, IConfiguration configuration)
        {
            this.tenantInfo = tenantInfo;
            this.configuration = configuration;
        }
        public string GetConnection()
        {
            var connectionString = configuration.GetConnectionString(this.tenantInfo.Name);
            if(string.IsNullOrEmpty(connectionString)){
                throw new NullReferenceException("can not find the connection");
            }
            return connectionString;
        }
    }
}