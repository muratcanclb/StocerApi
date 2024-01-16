using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Meb.Core;
using Meb.Data;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Intra.Api;
using Intra.Api.Domain.Dto;

namespace Intra.Api.Data
{
    public class PostgreContextFactory : MebDesignTimeDbContextFactory<PostgreContext>
    {
        public override PostgreContext CreateDbContext()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<PostgreContext>();

            string connectionString = Startup.ConfigurationPublic.GetConnectionString("PostgreSQL");

            dbContextOptionsBuilder.UseNpgsql(connectionString);
            var types = base.GetEntityTypeList<BaseEntity>(Assembly.GetExecutingAssembly(), new List<string> { "Intra.Api.Domain.Entities" });
            var myContext = new PostgreContext(dbContextOptionsBuilder.Options, types, false);
            return myContext;
        }
    }

    public class PostgreContext : MebObjectContext
    {
        public PostgreContext(DbContextOptions<PostgreContext> options, IEnumerable<Type> models, bool useMebEntityTypeConfiguration = false) : base(options, models, useMebEntityTypeConfiguration)
        {

        }
    }

    public class PostgreRepository<T> : EfRepository<T> where T : BaseEntity
    {
        public PostgreRepository(PostgreContext context) : base(context)
        {
        }

    }
}