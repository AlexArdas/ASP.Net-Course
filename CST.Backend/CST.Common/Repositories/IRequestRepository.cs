using CST.Common.Models.DTO;
using CST.Common.Models.Domain;
using CST.Common.Models.Enums;

namespace CST.Common.Repositories
{
    public interface IRequestRepository : IHasIdRepository<RequestDomainEntity>
    {
        Task<List<RequestResponse>> GetRequestsAsync(UserClaimModel currentUser);
        Task<RequestDomainEntity> GetRequestEntityByRequestIdAsync(Guid requestId, UserClaimModel currentUser);
        Task UpdateStatusAsync(Guid id, RequestStatus status);
        Task UpdateRequestAssigneeAsync(Guid requestId, Guid userId);
        Task<int> GetUnreadRequestsCountAsync(UserClaimModel currentUser);
    }
}
