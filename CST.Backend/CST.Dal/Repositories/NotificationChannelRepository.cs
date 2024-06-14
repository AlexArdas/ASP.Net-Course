using CST.Common.Models.Domain;
using CST.Common.Models.Enums;
using CST.Common.Repositories;
using CST.Dal.SqlContext;
using Microsoft.EntityFrameworkCore;

namespace CST.Dal.Repositories
{
    internal class NotificationChannelRepository : ItemHasIdRepository<NotificationChannelDomainEntity>, INotificationChannelRepository
    {
        public NotificationChannelRepository(ICstContextFactory dbFactory) : base(dbFactory) { }

        public async Task<NotificationChannelDomainEntity> UpdateNotificationChannelAsync(NotificationChannelDomainEntity domainEntity)
        {
            var context = DbFactory.CreateContext();
            context.Update(domainEntity);
            await context.SaveChangesAsync();

            return domainEntity;
        }

        /// <summary>
        /// Use it, if u need empty NotificationChannel with FK relation for example.
        /// </summary>
        /// <returns></returns>
        public NotificationChannelDomainEntity CreateDefaultNotificationChannel(Guid id)
        {
            return new NotificationChannelDomainEntity()
            {
                Id = id,
                Name = "not loaded",
                Rank = 0,
                Description = "not loaded",
                Frequency = "not loaded",
                Brief = "not loaded",
                IsPrivate = true,
                PersonalBlogScope = "not loaded",
                MailSubscribersCount = 0,
                PersonalBlogOwner = Guid.Empty,
                CreatedOn = DateTime.Now,
                TeamsLink = "not loaded"
            };
        }

        //Under this comment is all the old logic, which will be removed after approval from FE
        [Obsolete]
        public async Task<List<NotificationChannelDomainEntity>> GetNotificationChannelsByMailingsDateRangeAsync(DateTime? startDate, DateTime? endDate)
        {
            var context = DbFactory.CreateContext();

            var query = context.NotificationChannelDomainEntities.AsQueryable();
            query = query
                .Where(c => c.Mailings
                    .Any(mailing => (!startDate.HasValue || mailing.SendOn.Value >= startDate)
                                    && (!endDate.HasValue || mailing.SendOn.Value <= endDate)
                                    && !mailing.MailingStatus.Equals(MailingStatus.Cancelled)
                                    && !mailing.MailingStatus.Equals(MailingStatus.Draft)));

            return await query.ToListAsync();
        }
    }
}
