using CST.Common.Models.Domain;
using CST.Common.Repositories;
using CST.Dal.SqlContext;
using Microsoft.EntityFrameworkCore;

namespace CST.Dal.Repositories
{
    internal class MailingsApproversRepository : IMailingsApproversRepository
    {
        protected readonly ICstContextFactory DbFactory;
        public MailingsApproversRepository(ICstContextFactory cstContextFactory)
        {
            DbFactory = cstContextFactory;
        }

        public async Task ReplaceMailingApprovers(List<MailingsApproversDomainEntity> newApprovers)
        {
            var context = DbFactory.CreateContext();

            var mailingId = newApprovers.Select(x => x.MailingId).FirstOrDefault();

            var existingApprovers = await context.MailingsApproversDomainEntities
                .Where(x => x.MailingId == mailingId)
                .ToListAsync();

            context.MailingsApproversDomainEntities.RemoveRange(existingApprovers);

            await context.MailingsApproversDomainEntities.AddRangeAsync(newApprovers);

            await context.SaveChangesAsync();
        }
    }
}
