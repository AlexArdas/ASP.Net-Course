using CST.Common.Models.Enums;

namespace CST.Common.Models.DTO.Report
{
    public class ReportCell
    {
        public string Value { get; private set; }
        public ReportCellStyle CellStyle { get; private set; }

        public ReportCell(string value, ReportCellStyle cellStyle)
        {
            Value = value;
            CellStyle = cellStyle;
        }
    }
}
