using CST.Common.Models.Domain;
using CST.Common.Repositories;
using CST.Dal.SqlContext;
using Microsoft.EntityFrameworkCore;

namespace CST.Dal.Repositories
{
    internal class ItemHasIdRepository<T> : BaseRepository<T>, IHasIdRepository<T> where T : class, IHasId
    {
        public ItemHasIdRepository(ICstContextFactory hubContextFactory) : base(hubContextFactory) { }

        public virtual Task<T> GetItemByIdAsync(Guid id)
        {
            return DbFactory.CreateContext().Set<T>().FirstOrDefaultAsync(item => item.Id == id);
        }

        public virtual Task<bool> ExistsAsync(Guid id)
        {
            return DbFactory.CreateContext().Set<T>().AnyAsync(item => item.Id == id);
        }

        public virtual Task<List<T>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            return DbFactory.CreateContext().Set<T>()
                .Where(item => ids.Contains(item.Id))
                .ToListAsync();
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var context = DbFactory.CreateContext();
            var item =  await context.Set<T>().FirstOrDefaultAsync(item => item.Id == id);
            context.Set<T>().Remove(item);
            await context.SaveChangesAsync();
        }
    }
}
