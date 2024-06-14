using CST.Common.Models.Domain;
using CST.Common.Models.DTO;
using CST.Common.Models.Pagination;

namespace CST.Common.Repositories
{
    public interface IUserRepository : IHasIdRepository<UserDomainEntity>
    {
        Task<UserDomainEntity> GetUserByEmailAsync(string email);
        Task<UserResponse> GetUserInfoByIdAsync(Guid userId);
        Task<List<Guid>> GetUsersIdsByEmailsAsync(List<string> emails);
        Task<UserResponse> GetUserInfoByEmailAsync(string email);
        Task<UserDomainEntity> GetUserByExternalIdAsync(string externalId);
        Task<PaginatedList<UserBriefResponse>> FilterUsersByFullNameAsync(string searchOption, PaginationParameters paginationParameters);
    }
}
