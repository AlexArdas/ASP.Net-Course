using CST.Common.Attributes;

namespace CST.Common.Models.DTO.ReportResponse
{
    public class KeyNumberSetResponse
    {
        [IsNotReportable]
        public Guid Id { get; set; }
        public bool IncludeMailingsNumber { get; set; }
        
        public bool IncludeReadTime { get; set; }
        
        public bool IncludeOpenRate { get; set; }

        public bool IncludeRating { get; set; }
    }
}
