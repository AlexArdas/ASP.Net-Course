using CST.Common.Models.Domain;
using CST.Common.Models.DTO;
using CST.Common.Models.Enums;

namespace CST.Common.Services
{
    public interface IRequestService
    {
        Task<List<RequestResponse>> GetRequestsAsync();
        Task<RequestMessagesWithFormResponse> GetRequestMessagesWithFormAsync(Guid requestId);
        Task UpdateRequestStatusAsync(Guid id, RequestStatus requestsStatus);
        Task AssignRequestAsync(Guid requestId, Guid userId);
        Task<int> GetUnreadRequestsCountAsync();
        Task<RequestDomainEntity> CreateRequestAsync(RequestFormDomainEntity requestForm);
    }
}
