using CST.Common.Exceptions;
using CST.Common.Extensions;
using CST.Common.Models.Domain;
using CST.Common.Models.DTO;
using CST.Common.Models.Pagination;
using CST.Common.Repositories;
using CST.Dal.Extensions;
using CST.Dal.SqlContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CST.Dal.Repositories
{
    internal class UserRepository : ItemHasIdRepository<UserDomainEntity>, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ICstContextFactory dbFactory, ILogger<UserRepository> logger) : base(dbFactory)
        {
            _logger = logger;
        }

        public async Task<UserDomainEntity> GetUserByEmailAsync(string email)
        {
            return await DbFactory.CreateContext().UserDomainEntities
                .Include(u => u.UserRoles).ThenInclude(r => r.Role)
                .FirstOrDefaultAsync(u => u.Email.ToLower().Equals(email.Trim().ToLower()));
        }

        public async Task<UserResponse> GetUserInfoByIdAsync(Guid userId)
        {
            var user = await DbFactory.CreateContext().UserDomainEntities
                .Include(u => u.UserRoles).ThenInclude(r => r.Role)
                .Where(u => u.Id.Equals(userId))
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    JobTitle = u.JobTitle,
                    RoleNames = u.UserRoles.Select(r => r.Role.Name).ToList(),
                    AvatarUri = u.AvatarUri
                })
                .FirstOrDefaultAsync();
            if (user is null)
            {
                _logger.LogWarning($"GetUserInfoByIdAsync. User {userId} was not found");
                throw new NotFoundException($"User {userId} was not found");
            }
            return user;
        }

        public async Task<List<Guid>> GetUsersIdsByEmailsAsync(List<string> emails)
        {
            return await DbFactory.CreateContext().UserDomainEntities
                .Where(u => emails.Contains(u.Email.Trim().ToLower()))
                .Select(u => u.Id)
                .ToListAsync();
        }

        public async Task<UserResponse> GetUserInfoByEmailAsync(string email)
        {
            var user = await DbFactory.CreateContext().UserDomainEntities
                .Include(u => u.UserRoles).ThenInclude(r => r.Role)
                .Where(u => u.Email.ToLower().Equals(email.Trim().ToLower()))
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    JobTitle = u.JobTitle,
                    RoleNames = u.UserRoles.Select(r => r.Role.Name).ToList(),
                    AvatarUri = u.AvatarUri
                })
                .FirstOrDefaultAsync();

            if (user is null)
            {
                throw new NotFoundException($"User with email {email} was not found.");
            }

            return user;
        }

        public async Task<UserDomainEntity> GetUserByExternalIdAsync(string externalId)
        {
            return await DbFactory.CreateContext().UserDomainEntities
                .Where(u => u.ExternalId.Equals(externalId))
                .FirstOrDefaultAsync();
        }

        public async Task<PaginatedList<UserBriefResponse>> FilterUsersByFullNameAsync(string searchOption, PaginationParameters paginationParameters)
        {
            var users = DbFactory.CreateContext().UserDomainEntities.AsQueryable();

            if (!searchOption.IsNullOrEmpty())
            {
                users = users.Where(u => u.FullName.ToLower().Contains(searchOption.ToLower()));
            }

            var userBriefs = await users.Paginate(paginationParameters)
                .Select(u => new UserBriefResponse
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    JobTitle = u.JobTitle,
                    AvatarUri = u.AvatarUri
                }).ToListAsync();

            return new PaginatedList<UserBriefResponse>(userBriefs, await users.CountAsync());
        }
    }
}
