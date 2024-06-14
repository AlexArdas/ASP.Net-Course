namespace CST.Common.Models.DTO
{
    public class ReportBriefViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int FileSize { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Uri Uri { get; set; }
    }
}
