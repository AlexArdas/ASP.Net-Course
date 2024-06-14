using CST.Common.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CST.Dal.EntityTypeConfigurations
{
    public class UserDomainEntityConfiguration : IEntityTypeConfiguration<UserDomainEntity>
    {
        public void Configure(EntityTypeBuilder<UserDomainEntity> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Email).IsRequired();
            builder.Property(u => u.FullName).IsRequired();
            builder.Property(u => u.LocationId).IsRequired();
            builder.Property(u => u.ExternalId).IsRequired();
            builder.Property(u => u.DoB).HasColumnType("date");
            builder.Property(u => u.Gender).HasConversion<string>();
            builder.HasIndex(u => u.Id);
            builder.HasIndex(u => u.LocationId);
            builder.HasIndex(u => u.ManagerId);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasOne(u => u.Location)
                .WithMany()
                .HasForeignKey(l => l.LocationId);

        }
    }
}
