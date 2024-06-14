using CST.Common.Models.Domain;

namespace CST.Common.Repositories
{
    public interface IShownRequestToUserRepository : IRepository<ShownRequestToUserDomainEntity>
    {
        Task RemoveByRequestIdsAsync(List<Guid> requestIds);
    }
}
