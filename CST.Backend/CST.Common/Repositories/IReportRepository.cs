using CST.Common.Models.Domain;
using CST.Common.Models.DTO.ReportResponse;

namespace CST.Common.Repositories
{
    public interface IReportRepository : IHasIdRepository<ReportDomainEntity>
    {
        Task<ReportDomainEntity> UpdateReportAsync(ReportDomainEntity report);
        Task<ReportResponse> GetReportAsync(Guid id);
        Task<ReportDomainEntity> UpdateReportSizeAndUriAsync(Guid id, Uri uri, int size);
        Task<List<ReportDomainEntity>> GetReportsWithUri();
    }
}
