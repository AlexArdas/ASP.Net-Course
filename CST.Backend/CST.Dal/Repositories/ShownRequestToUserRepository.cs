using CST.Common.Models.Domain;
using CST.Common.Repositories;
using CST.Dal.SqlContext;
using Microsoft.EntityFrameworkCore;

namespace CST.Dal.Repositories
{
    internal class ShownRequestToUserRepository : BaseRepository<ShownRequestToUserDomainEntity>, IShownRequestToUserRepository
    {
        public ShownRequestToUserRepository(ICstContextFactory dbFactory) : base(dbFactory)
        { }

        public async Task RemoveByRequestIdsAsync(List<Guid> requestIds)
        {
            var context = DbFactory.CreateContext();
            var shownRequestsToUser = await context.ShownRequestToUserDomainEntities
                .Where(rr => requestIds.Contains(rr.RequestId)).ToListAsync();
            context.ShownRequestToUserDomainEntities.RemoveRange(shownRequestsToUser);
            await context.SaveChangesAsync();
        }
    }
}
