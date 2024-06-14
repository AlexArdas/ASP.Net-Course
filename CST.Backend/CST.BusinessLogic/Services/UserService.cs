using CST.Common.Models.Domain;
using CST.Common.Models.DTO;
using CST.Common.Models.Pagination;
using CST.Common.Repositories;
using CST.Common.Services;

namespace CST.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserDomainEntity> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task<UserResponse> GetUserInfoByIdAsync(Guid userId)
        {
            return await _userRepository.GetUserInfoByIdAsync(userId);
        }

        public async Task<UserResponse> GetUserInfoByEmailAsync(string email)
        {
            return await _userRepository.GetUserInfoByEmailAsync(email);
        }

        public async Task<PaginatedList<UserBriefResponse>> FilterUsersByFullNameAsync(string searchOption, PaginationParameters paginationParameters)
        { 
            return await _userRepository.FilterUsersByFullNameAsync(searchOption, paginationParameters);
        }
    }
}
