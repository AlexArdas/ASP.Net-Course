using CST.Common.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CST.Dal.EntityTypeConfigurations
{
    public class UserRoleDomainEntityConfiguration : IEntityTypeConfiguration<UserRoleDomainEntity>
    {
        public void Configure(EntityTypeBuilder<UserRoleDomainEntity> builder)
        {
            builder.HasKey(ur => new { ur.RoleId, ur.UserId });
            builder.HasIndex(ur => ur.RoleId);

            builder.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur=> ur.RoleId);

            builder.HasOne(ua => ua.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ua => ua.UserId);

        }
    }
}
