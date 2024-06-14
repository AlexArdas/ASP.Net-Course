using CST.Common.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CST.Dal.EntityTypeConfigurations
{
    public  class RoleDomainEntityConfiguration :  IEntityTypeConfiguration<RoleDomainEntity> 
    {
        public void Configure(EntityTypeBuilder<RoleDomainEntity> builder)
        {
            builder.Property(r => r.Name).HasConversion<string>().IsRequired();
            builder.Property(r => r.Description).IsRequired();

        }
    }
}
