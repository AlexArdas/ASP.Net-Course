using CST.Common.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CST.Dal.EntityTypeConfigurations
{
    public class NotificationChannelDomainEntityConfiguration : IEntityTypeConfiguration<NotificationChannelDomainEntity>
    {
        public void Configure(EntityTypeBuilder<NotificationChannelDomainEntity> builder)
        {
            builder.Property(n => n.Name).IsRequired();
            builder.Property(n => n.Description).IsRequired();
            builder.Property(n => n.Frequency).IsRequired();
            builder.Property(n => n.Brief).IsRequired();
            builder.Property(n => n.PersonalBlogScope);
            builder.Property(n => n.TeamsLink).IsRequired();
        }
    }
}
