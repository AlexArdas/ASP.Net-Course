namespace CST.Common.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<int> GetCountAsync();
        Task<bool> AnyAsync();
        Task<T> AddAsync(T item);
        Task AddRangeAsync(IEnumerable<T> item);
        Task<List<T>> GetAllAsync();
    }
}
