using CST.Common.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CST.Dal.EntityTypeConfigurations
{
    class RequestMessageDomainEntityConfiguration : IEntityTypeConfiguration<RequestMessageDomainEntity>
    {
        public void Configure(EntityTypeBuilder<RequestMessageDomainEntity> builder)
        {
            builder.Property(x => x.Body).IsRequired();
            builder.Property(x => x.CreatedOn).IsRequired();
            builder.Property(x => x.ModifiedOn).IsRequired();
            builder.HasOne(x => x.Request).WithMany(r=>r.RequestMessage).OnDelete(DeleteBehavior.Restrict);
        }
    }
}