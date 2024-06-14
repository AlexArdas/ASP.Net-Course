using CST.Common.Models.Enums;

namespace CST.Common.Models.DTO.Report
{
    public class ReportData
    {
        public List<ReportRow> Rows { get; private set; }

        private ReportRow CurrentRow { get; set; }

        public ReportData()
        {
            Rows = new List<ReportRow>();
        }

        public void CreateRow()
        {
            CurrentRow = new ReportRow();
            Rows.Add(CurrentRow);
        }

        public void CreateCell(string value, ReportCellStyle cellStyle)
        {
            var cell = new ReportCell(value, cellStyle);
            CurrentRow.Cells.Add(cell);
        }
    }
}
