using CST.Common.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CST.Dal.EntityTypeConfigurations
{
    public class MailingsApproversDomainEntityConfiguration : IEntityTypeConfiguration<MailingsApproversDomainEntity>
    {
        public void Configure(EntityTypeBuilder<MailingsApproversDomainEntity> builder)
        {
            builder.HasKey(a => new { a.MailingId, a.ApproverId });
            builder.HasOne(m => m.Mailing).WithMany(n => n.MailingsApprovers)
                .HasForeignKey(n => n.MailingId).IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(m => m.Approver).WithMany(u => u.MailingApprovers)
                .HasForeignKey(n => n.ApproverId).IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
