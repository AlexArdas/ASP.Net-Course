using CST.Common.Models.Enums;

namespace CST.Common.Models.DTO.ReportRequest
{
    public class ReportRequest
    {
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        
        public List<Guid> MailingIds { get; set; }

        public KeyNumberSetRequest KeyNumberSet { get; set; }

        public ReportColumnSetRequest ReportColumnSet { get; set; }

        public ReportGroupByOption? GroupBy { get; set; }

        public string? SortByField { get; set; }

        public SortOrder? SortOrder { get; set; }
    }
}