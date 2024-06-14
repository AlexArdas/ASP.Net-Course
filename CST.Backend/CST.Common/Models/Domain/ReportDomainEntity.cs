using CST.Common.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CST.Common.Models.Domain
{
    [Table("Report")]
    public sealed class ReportDomainEntity : IHasId
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int FileSize { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Uri Uri { get; set; }

        public List<Guid> MailingIds { get; set; }
        
        public List<MailingDomainEntity> Mailings { get; set; }

        public KeyNumberSetDomainEntity KeyNumberSet { get; set; }

        public ReportColumnSetDomainEntity ReportColumnSet { get; set; }

        public ReportGroupByOption? GroupBy { get; set; }

        public string? SortByField { get; set; }

        public SortOrder? SortOrder { get; set; }
    }
}
