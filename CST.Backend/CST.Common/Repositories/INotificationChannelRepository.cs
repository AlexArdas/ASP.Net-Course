using CST.Common.Models.Domain;
using CST.Common.Models.DTO;

namespace CST.Common.Repositories
{
    public interface INotificationChannelRepository : IHasIdRepository<NotificationChannelDomainEntity>
    {
        Task<NotificationChannelDomainEntity> UpdateNotificationChannelAsync(NotificationChannelDomainEntity domainEntity);

        NotificationChannelDomainEntity CreateDefaultNotificationChannel(Guid id);

        //Under this comment is all the old logic, which will be removed after approval from FE
        Task<List<NotificationChannelDomainEntity>> GetNotificationChannelsByMailingsDateRangeAsync(DateTime? startDate, DateTime? endDate);
    }
}
