using CST.Common.Models.Domain;

namespace CST.Common.Repositories
{
    public interface IHasIdRepository<T> : IRepository<T> where T : class, IHasId
    {
        Task<T> GetItemByIdAsync(Guid id);

        Task<bool> ExistsAsync(Guid id);
        
        Task<List<T>> GetByIdsAsync(IEnumerable<Guid> ids);
        
        Task DeleteAsync(Guid id);
    }
}
