namespace CST.Common.Models.DTO.ReportRequest
{
    public class KeyNumberSetRequest
    {
        public bool IncludeMailingsNumber { get; set; }
        
        public bool IncludeReadTime { get; set; }
        
        public bool IncludeOpenRate { get; set; }

        public bool IncludeRating { get; set; }
    }
}