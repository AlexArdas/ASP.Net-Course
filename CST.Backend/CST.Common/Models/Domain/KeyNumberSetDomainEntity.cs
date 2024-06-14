using System.ComponentModel.DataAnnotations.Schema;
using CST.Common.Attributes;

namespace CST.Common.Models.Domain
{
    [Table("KeyNumberSet")]
    public sealed class KeyNumberSetDomainEntity : IHasId
    {
        [IsNotReportable]
        public Guid Id { get; set; }
        [IsNotReportable]
        public ReportDomainEntity Report { get; set; }

        public bool IncludeReadTime { get; set; }

        public bool IncludeMailingsNumber { get; set; }

        public bool IncludeOpenRate { get; set; }

        public bool IncludeRating { get; set; }
    }
}
