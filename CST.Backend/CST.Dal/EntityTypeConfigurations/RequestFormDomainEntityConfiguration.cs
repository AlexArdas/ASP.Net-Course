using CST.Common.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CST.Dal.EntityTypeConfigurations
{
    public class RequestFormDomainEntityConfiguration : IEntityTypeConfiguration<RequestFormDomainEntity>
    {
        public void Configure(EntityTypeBuilder<RequestFormDomainEntity> builder)
        {
            builder.Property(r => r.Name).IsRequired();
            builder.Property(r => r.Description).IsRequired();
            builder.Property(r => r.From).IsRequired();
            builder.Property(r => r.Recipients).IsRequired();
            builder.Property(r => r.Customer).IsRequired();
            builder.Property(r => r.ExpectedSendDate).IsRequired();
            builder.Property(r => r.LinkToFilesAtOnedrive).IsRequired();
            builder.Property(r => r.RequesterEmail).IsRequired();
            builder.HasOne(x => x.Request)
                .WithOne(r => r.RequestForm)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
