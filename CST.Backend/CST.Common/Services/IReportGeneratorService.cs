using System.Data;
using CST.Common.Models.DTO.Report;
using CST.Common.Models.DTO.ReportResponse;
using CST.Common.Models.Enums;

namespace CST.Common.Services
{
    public interface IReportGeneratorService
    {
        byte[] ExcelConverter(ReportData reportData);
        
        Task<ReportData> FillReportData(ReportResponse reportResponse);
    }
}
