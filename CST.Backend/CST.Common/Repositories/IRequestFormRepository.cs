using CST.Common.Models.Domain;

namespace CST.Common.Repositories
{
    public interface IRequestFormRepository : IHasIdRepository<RequestFormDomainEntity>
    {
        Task<RequestFormDomainEntity> UpdateRequestFormAsync(RequestFormDomainEntity requestForm);
    }
}
