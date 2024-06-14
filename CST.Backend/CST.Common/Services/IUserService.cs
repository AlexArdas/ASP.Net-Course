using CST.Common.Models.Domain;
using CST.Common.Models.DTO;
using CST.Common.Models.Pagination;

namespace CST.Common.Services
{
    public interface IUserService
    {
        Task<UserDomainEntity> GetUserByEmailAsync(string email);
        Task<UserResponse> GetUserInfoByIdAsync(Guid userId);
        Task<UserResponse> GetUserInfoByEmailAsync(string userEmail);
        Task<PaginatedList<UserBriefResponse>> FilterUsersByFullNameAsync(string searchOption, PaginationParameters paginationParameters);
    }
}
