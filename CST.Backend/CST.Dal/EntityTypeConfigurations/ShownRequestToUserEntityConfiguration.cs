using CST.Common.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CST.Dal.EntityTypeConfigurations
{
    public class ShownRequestToUserEntityConfiguration : IEntityTypeConfiguration<ShownRequestToUserDomainEntity>
    {
        public void Configure(EntityTypeBuilder<ShownRequestToUserDomainEntity> builder)
        {
            builder.HasKey(rr => new { rr.UserId, rr.RequestId });
            builder.HasIndex(rr => rr.UserId);

            builder.HasOne(rr => rr.User)
                .WithMany(u => u.ShownRequestsToUser)
                .HasForeignKey(rr => rr.UserId);

            builder.HasOne(rr => rr.Request)
                .WithMany(r => r.RequestReadingsByUser)
                .HasForeignKey(rr => rr.RequestId);
        }
    }
}
