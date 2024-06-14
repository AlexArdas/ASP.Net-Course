using CST.Common.Models.Domain;
using CST.Common.Models.DTO;

namespace CST.Common.Repositories
{
    public interface ILocationRepository : IHasIdRepository<LocationDomainEntity>
    {
        Task<Dictionary<Guid, string>> GetMailingsLocationNamesAsync(List<MailingReportResponse> mailings);
    }
}
