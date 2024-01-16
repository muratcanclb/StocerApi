using Autofac;
using System;
using System.Collections.Generic;
using System.Reflection;
using Meb.Api.Framework.Infrastructure;
using Meb.Core;
using Meb.Core.Configuration;
using Meb.Core.Infrastructure;
using Meb.Core.Infrastructure.DependencyManagement;
using Meb.Data;
using Intra.Api.Data;
using Microsoft.EntityFrameworkCore;
using Intra.Api.Domain.Maps;
using Microsoft.Extensions.Configuration;

namespace Intra.Api.Infrastructure
{
    /// <summary>
    /// Dependency registrar
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, MebConfig config)
        {

            #region Postgre Sql Deep Cell Zoom Db

            var postgreNameSpaceList = new List<string>() { "Intra.Api.Domain.Entities" };
            builder.RegisterRepository(typeof(PostgreRepository<>), typeof(PostgreRepository<>));
            builder.Register(x => { return new PostgreContextFactory().CreateDbContext(); }).As<PostgreContext>();

            var postgreConnectionString = Startup.ConfigurationPublic.GetConnectionString("PostgreSQL");
            var dbContextOptionsBuilder = new DbContextOptionsBuilder();
            dbContextOptionsBuilder.UseNpgsql(postgreConnectionString);

            #endregion


            //Common Model(LogModel, SettingModel) Add.
            var PublicModels = new List<Type> { typeof(LogMap), typeof(SettingMap) };
            builder.Register<IDbContext>(c => new MebObjectContext(dbContextOptionsBuilder.Options, PublicModels, true)).InstancePerLifetimeScope();
        }

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public int Order
        {
            get { return 2; }
        }
    }
}