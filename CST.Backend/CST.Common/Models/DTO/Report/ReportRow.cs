namespace CST.Common.Models.DTO.Report
{
    public class ReportRow
    {
        public List<ReportCell> Cells { get; set; }

        public ReportRow()
        {
            Cells = new List<ReportCell>();
        }
    }
}
