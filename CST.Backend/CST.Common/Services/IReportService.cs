using CST.Common.Models.DTO;
using CST.Common.Models.DTO.ReportRequest;
using CST.Common.Models.DTO.ReportResponse;

namespace CST.Common.Services
{
    public interface IReportService
    {
        Task<List<ReportBriefViewModel>> GetReportsAsync();
        Task<ReportResponse> CreateReportEntityAsync(ReportRequest reportDto);
        Task<byte[]> GenerateReportAsync(ReportResponse reportDto);
        Task<ReportResponse> CreateReportAsync(ReportRequest reportDto);
    }
}
