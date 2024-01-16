using Meb.Core.Domain.Configuration;
using Meb.Data.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Intra.Api.Domain.Maps
{
    /// <summary>
    /// Mapping class of Setting
    /// </summary>
    public partial class SettingMap : MebEntityTypeConfiguration<Setting>
    {
        public override void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder.ToTable("Setting", "DEMO");
            builder.HasKey(s => s.Id);
            builder.Property(t => t.Id).HasColumnName("ID");
            builder.Property(s => s.Name).HasColumnName("NAME").IsRequired().HasMaxLength(200);
            builder.Property(s => s.Value).HasColumnName("VALUE").IsRequired().HasMaxLength(2000);
            builder.Property(s => s.StoreId).HasColumnName("STORE_ID").IsRequired();
            base.Configure(builder);
        }
    }
}