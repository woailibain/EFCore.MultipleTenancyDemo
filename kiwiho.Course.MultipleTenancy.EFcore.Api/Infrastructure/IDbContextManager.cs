using System;
using Microsoft.EntityFrameworkCore;

namespace kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure
{
    public interface IDbContextManager
    {
        DatabaseIntegration DatabaseType { get; }
        DbContextOptionsBuilder GenerateOptionBuilder(DbContextOptionsBuilder builder, string connectionString);
    }

    public class MySqlDbContextManager : IDbContextManager
    {
        public DatabaseIntegration DatabaseType => DatabaseIntegration.Mysql;

        public DbContextOptionsBuilder GenerateOptionBuilder(DbContextOptionsBuilder builder, string connectionString)
        {
            return builder.UseMySql(connectionString);
        }
    }

    public class SqlServerDbContextManager : IDbContextManager
    {
        public DatabaseIntegration DatabaseType => DatabaseIntegration.SqlServer;
        public DbContextOptionsBuilder GenerateOptionBuilder(DbContextOptionsBuilder builder, string connectionString)
        {
            return builder.UseSqlServer(connectionString);
        }
    }

    public enum DatabaseIntegration
    {
        None = 0,
        Mysql = 1,
        SqlServer = 2
    }
}
