using Meb.Core;
using Meb.Core.Data;
using Meb.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Intra.Api.Data
{

    public class DeepCellPostgreContext : MebObjectContext, IDeepCellPostgreContext
    {
        public DeepCellPostgreContext(
            DbContextOptions<DeepCellPostgreContext> options,
            List<Type> models,
            bool useMebEntityTypeConfiguration = false) : base(options, models, useMebEntityTypeConfiguration)
        {

        }
    }

    public class DeepCellPostgreRepository<T> : EfRepository<T>, IDeepCellPostgreRepository<T> where T : BaseEntity
    {
        public DeepCellPostgreRepository(IDeepCellPostgreContext context) : base(context)
        {

        }
    }

    public interface IDeepCellPostgreContext : IDbContext
    {

    }

    public interface IDeepCellPostgreRepository<T> : IRepository<T> where T : BaseEntity
    {

    }
}
