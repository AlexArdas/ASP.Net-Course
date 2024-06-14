using CST.Common.Repositories;
using CST.Dal.SqlContext;
using Microsoft.EntityFrameworkCore;

namespace CST.Dal.Repositories
{
    internal class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly ICstContextFactory DbFactory;

        public BaseRepository(ICstContextFactory cstContextFactory)
        {
            DbFactory = cstContextFactory;
        }

        public virtual Task<int> GetCountAsync()
        {
            return DbFactory.CreateContext().Set<T>().CountAsync();
        }

        public virtual Task<T> AddAsync(T item)
        {
            var dbContext = DbFactory.CreateContext();
            dbContext.Set<T>().Add(item); 
            dbContext.SaveChangesAsync();
            return Task.FromResult(item);
        }

        public virtual Task AddRangeAsync(IEnumerable<T> item)
        {
            var dbContext = DbFactory.CreateContext();
            dbContext.Set<T>().AddRangeAsync(item);
            dbContext.SaveChangesAsync();
            return Task.FromResult(item);
        }

        public virtual Task<List<T>> GetAllAsync()
        {
            return DbFactory.CreateContext().Set<T>().ToListAsync();
        }

        public virtual Task<bool> AnyAsync()
        {
             return DbFactory.CreateContext().Set<T>().AnyAsync();
        }
    }
}
