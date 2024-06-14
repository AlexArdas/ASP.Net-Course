using CST.Common.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CST.Dal.EntityTypeConfigurations
{
    public class LocationDomainEntityConfiguration : IEntityTypeConfiguration<LocationDomainEntity>
    {
        public void Configure(EntityTypeBuilder<LocationDomainEntity> builder)
        {
            builder.HasKey(l => l.Id);
            builder.Property(l => l.ExternalId).IsRequired();
            builder.Property(l => l.Name).IsRequired();
            builder.Property(l => l.Type).IsRequired();
            builder.Property(n => n.Type).HasConversion<string>();
            builder.HasIndex(n => n.ExternalId).IsUnique();
        }
    }
}
