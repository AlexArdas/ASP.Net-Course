using CST.Common.Models.Domain;
using CST.Common.Models.DTO;
using CST.Common.Repositories;
using CST.Dal.SqlContext;
using Microsoft.EntityFrameworkCore;

namespace CST.Dal.Repositories
{
    internal class LocationRepository : ItemHasIdRepository<LocationDomainEntity>, ILocationRepository
    {
        public LocationRepository(ICstContextFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<Dictionary<Guid, string>> GetMailingsLocationNamesAsync(List<MailingReportResponse> mailings)
        {
            var context = DbFactory.CreateContext();
            var locationsQuery = context.LocationDomainEntities.AsQueryable();
            var mailingsLocations = new Dictionary<Guid, string>();
            foreach (var mailing in mailings)
            {
                var mailingLocationsIds = mailing.MailingLocations;
                var names = await locationsQuery
                    .Where(l => mailingLocationsIds.Contains(l.Id))
                    .Select(l => l.Name)
                    .ToListAsync();
                mailingsLocations.Add(mailing.Id, string.Join(", ", names));
            }
            return mailingsLocations;
        }
    }
}
