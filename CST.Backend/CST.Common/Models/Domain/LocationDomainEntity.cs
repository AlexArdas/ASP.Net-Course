using CST.Common.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CST.Common.Models.Domain
{
    [Table("Location")]
    public class LocationDomainEntity : IHasId
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ExternalId { get; set; }
        public LocationType Type { get; set; }
        public int? Timezone { get; set; }
        public Guid? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public virtual LocationDomainEntity ParentLocation { get; set; }
    }
}
