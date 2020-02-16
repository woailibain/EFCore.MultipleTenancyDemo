using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;

namespace kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure
{
    public class MigrationByTenantAssembly : MigrationsAssembly
    {
        private readonly DbContext context;

        public MigrationByTenantAssembly(ICurrentDbContext currentContext,
              IDbContextOptions options, IMigrationsIdGenerator idGenerator,
              IDiagnosticsLogger<DbLoggerCategory.Migrations> logger)
          : base(currentContext, options, idGenerator, logger)
        {
            context = currentContext.Context;
        }

        public override Migration CreateMigration(TypeInfo migrationClass,
              string activeProvider)
        {
            if (activeProvider == null)
                throw new ArgumentNullException($"{nameof(activeProvider)} argument is null");

            var hasCtorWithSchema = migrationClass
                    .GetConstructor(new[] { typeof(string) }) != null;

            if (hasCtorWithSchema && context is ITenantDbContext tenantDbContext)
            {
                var instance = (Migration)Activator.CreateInstance(migrationClass.AsType(), tenantDbContext?.TenantInfo?.Name);
                instance.ActiveProvider = activeProvider;
                return instance;
            }

            return base.CreateMigration(migrationClass, activeProvider);
        }
    }
}
