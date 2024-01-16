using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Meb.Core.Domain.Logging;
using Meb.Data.Mapping;

namespace Intra.Api.Domain.Maps
{
    /// <summary>
    /// Mapping class of Log
    /// </summary>
    public partial class LogMap : MebEntityTypeConfiguration<Log>
    {
        public override void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.ToTable("LOG", "DEMO");
            builder.HasKey(l => l.Id);
            builder.Property(t => t.Id).HasColumnName("ID");
            builder.Property(l => l.LogLevelId).HasColumnName("LOG_LEVEL_ID").IsRequired();
            builder.Property(l => l.ShortMessage).HasColumnName("SHORT_MESSAGE").IsRequired();
            builder.Property(l => l.FullMessage).HasColumnName("FULL_MESSAGE");
            builder.Property(l => l.IpAddress).HasColumnName("IP_ADDRESS").HasMaxLength(200);
            builder.Property(l => l.CustomerId).HasColumnName("CUSTOMER_ID");
            builder.Property(l => l.PageUrl).HasColumnName("PAGE_URL");
            builder.Property(l => l.ReferrerUrl).HasColumnName("REFERRER_URL");
            builder.Property(l => l.CreatedOnUtc).HasColumnName("CREATED_ON_UTC").IsRequired();
            builder.Property(l => l.ExecuteTime).HasColumnName("EXECUTE_TIME");
            builder.Ignore(l => l.LogLevel);
            base.Configure(builder);
        }
    }
}