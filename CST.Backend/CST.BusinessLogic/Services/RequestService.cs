using CST.Common.Models.DTO;
using CST.Common.Repositories;
using CST.Common.Services;
using AutoMapper;
using CST.Common.Exceptions;
using CST.Common.Models.Enums;
using CST.Common.Models.Domain;
using Microsoft.Extensions.Logging;

namespace CST.BusinessLogic.Services
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IUserRepository _userRepository;
        private readonly IShownRequestToUserRepository _shownRequestToUserRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RequestService> _logger;

        public RequestService(IRequestRepository requestRepository,
            IUserRepository userRepository,
            IShownRequestToUserRepository shownRequestToUserRepository,
            IMapper mapper,
            ILogger<RequestService> logger)
        {
            _requestRepository = requestRepository;
            _userRepository = userRepository;
            _shownRequestToUserRepository = shownRequestToUserRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RequestDomainEntity> CreateRequestAsync(RequestFormDomainEntity requestForm)
        {
            var request = _mapper.Map<RequestDomainEntity>(requestForm);
            request.RequestStatus = RequestStatus.Created;

            await _requestRepository.AddAsync(request);

            return request;
        }

        public async Task<List<RequestResponse>> GetRequestsAsync()
        {
            var currentUser = new UserClaimModel{Email = "test@mail.ru"};
            return await _requestRepository.GetRequestsAsync(currentUser);
        }

        public async Task<RequestMessagesWithFormResponse> GetRequestMessagesWithFormAsync(Guid requestId)
        {
            var currentUser = new UserClaimModel { Email = "test@mail.ru" };
            var requestEntity = await _requestRepository.GetRequestEntityByRequestIdAsync(requestId, currentUser);

            if (requestEntity is null)
            {
                throw new NotFoundException($"Request {requestId} not found");
            }
            await MarkRequestAsReadByUser(requestId, currentUser.Id);

            var requestMessages = requestEntity.RequestMessage
                .OrderBy(r => r.CreatedOn).ToList();

            return new RequestMessagesWithFormResponse()
            {
                RequestFormResponse = _mapper.Map<RequestFormResponse>(requestEntity.RequestForm),
                RequestMessagesResponse = _mapper.Map<List<RequestMessageResponse>>(requestMessages)
            };
        }

        public async Task UpdateRequestStatusAsync(Guid id, RequestStatus requestStatus)
        {
            var request = await _requestRepository.GetItemByIdAsync(id);
            if (request is null)
            {
                _logger.LogWarning($"UpdateRequestStatusAsync. Request {id} was not found");
                throw new NotFoundException($"Request {id} was not found");
            }
            if (CanCurrentUserUpdateRequest(request))
            {
                await _requestRepository.UpdateStatusAsync(id, requestStatus);
            }
            else
            {
                throw new ForbidException();
            }
        }

        public async Task AssignRequestAsync(Guid requestId, Guid userId)
        {
            var request = await _requestRepository.GetItemByIdAsync(requestId);
            if (request is null)
            {
                _logger.LogWarning($"AssignRequestAsync. Request {requestId} was not found");
                throw new NotFoundException($"Request {requestId} was not found");
            }
            var assignee = await _userRepository.GetUserInfoByIdAsync(userId);
            if (CanCurrentUserUpdateRequest(request)
                && IsUserCommunicationTeamMember(assignee))
            {
                await _requestRepository.UpdateRequestAssigneeAsync(requestId, userId);
            }
            else
            {
                throw new ForbidException();
            }
        }

        public async Task<int> GetUnreadRequestsCountAsync()
        {
            var currentUser = new UserClaimModel { Email = "test@mail.ru" };
            return await _requestRepository.GetUnreadRequestsCountAsync(currentUser);
        }

        private static bool IsUserCommunicationTeamMember(UserResponse user)
        {
            var userRoles = user.RoleNames;
            return userRoles.Contains(RoleNames.CstHubAdmin)
                || userRoles.Contains(RoleNames.CstMccManager);
        }

        private bool CanCurrentUserUpdateRequest(RequestDomainEntity request)
        {
            var currentUser = new UserClaimModel { Email = "test@mail.ru" };
            var userRoles = currentUser.RoleNames;
            _ = userRoles ?? throw new NullReferenceException($"{nameof(userRoles)} shouldn't be null");
            return userRoles.Contains(RoleNames.CstHubAdmin)
                || userRoles.Contains(RoleNames.CstMccManager)
                && request.AssigneeId == currentUser.Id;
        }

        private async Task MarkRequestAsReadByUser(Guid requestId, Guid userId)
        {
            await _shownRequestToUserRepository
                .AddAsync(new ShownRequestToUserDomainEntity
                {
                    UserId = userId,
                    RequestId = requestId
                });
        }
    }
}
