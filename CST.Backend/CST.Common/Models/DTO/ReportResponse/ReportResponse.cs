using CST.Common.Models.Enums;

namespace CST.Common.Models.DTO.ReportResponse
{
    public class ReportResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public int FileSize { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        
        public List<Guid> MailingIds { get; set; }

        public List<MailingReportResponse> Mailings { get; set; }

        public KeyNumberSetResponse KeyNumberSet { get; set; }

        public ReportColumnSetResponse ReportColumnSet { get; set; }
        
        public Uri Uri { get; set; }

        public ReportGroupByOption? GroupBy { get; set; }

        public string? SortByField { get; set; }

        public SortOrder? SortOrder { get; set; }
    }
}
