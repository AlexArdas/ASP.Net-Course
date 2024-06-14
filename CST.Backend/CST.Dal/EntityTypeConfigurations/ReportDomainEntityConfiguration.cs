using CST.Common.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CST.Dal.EntityTypeConfigurations
{
    public class ReportDomainEntityConfiguration : IEntityTypeConfiguration<ReportDomainEntity>
    {
        public void Configure(EntityTypeBuilder<ReportDomainEntity> builder)
        {
            builder.Property(r => r.GroupBy).HasConversion<string>();

            builder.Property(r => r.SortOrder).HasConversion<string>();

            builder.HasOne(r => r.KeyNumberSet).WithOne(k => k.Report)
                .HasForeignKey<KeyNumberSetDomainEntity>(k => k.Id);

            builder.HasOne(r => r.ReportColumnSet).WithOne(c => c.Report)
                .HasForeignKey<ReportColumnSetDomainEntity>(c => c.Id);

            builder.HasMany(p => p.Mailings)
                .WithMany(p => p.Reports)
                .UsingEntity(j => j.ToTable("MailingReport"));

            builder.Property(r => r.Name).IsRequired();
        }
    }
}
