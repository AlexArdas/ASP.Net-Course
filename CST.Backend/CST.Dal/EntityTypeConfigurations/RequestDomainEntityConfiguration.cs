using CST.Common.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CST.Dal.EntityTypeConfigurations
{
    public class RequestDomainEntityConfiguration : IEntityTypeConfiguration<RequestDomainEntity>
    {
        public void Configure(EntityTypeBuilder<RequestDomainEntity> builder)
        {
            builder.Property(x => x.CreatedOn).IsRequired();
            builder.Property(x => x.RequesterEmail).IsRequired();
            builder.Property(x => x.RequestStatus).HasConversion<string>().IsRequired();
        }
    }
}
