using CST.Common.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CST.Dal.EntityTypeConfigurations
{
    public class MailingDomainEntityConfiguration : IEntityTypeConfiguration<MailingDomainEntity>
    {
        public void Configure(EntityTypeBuilder<MailingDomainEntity> builder)
        {
            builder.Property(m => m.MailingStatus).HasConversion<string>().IsRequired();
            builder.Property(m => m.AuthorId).IsRequired(false);
            builder.Property(m => m.Subject).IsRequired();
            builder.Property(m => m.SendOn).IsRequired(false);
            builder.Property(m => m.DeletedOn).IsRequired(false);
            builder.HasOne(m => m.Channel).WithMany(n => n.Mailings)
                .HasForeignKey(n => n.ChannelId).IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(m => m.Author).WithMany(u => u.Mailings)
                .HasForeignKey(n => n.AuthorId).IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
