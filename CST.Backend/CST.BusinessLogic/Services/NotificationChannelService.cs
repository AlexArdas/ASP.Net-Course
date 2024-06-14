using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using CST.Common.Models.Domain;
using CST.Common.Models.DTO;
using CST.Common.Models.Messages;
using CST.Common.Repositories;
using CST.Common.Services;

namespace CST.BusinessLogic.Services
{
    public class NotificationChannelService : INotificationChannelService
    {
        private readonly INotificationChannelRepository _notificationChannelRepository;
        private readonly IMapper _mapper;

        public NotificationChannelService(INotificationChannelRepository notificationChannelRepository, IMapper mapper)
        {
            _notificationChannelRepository = notificationChannelRepository;
            _mapper = mapper;
        }

        public async Task<NotificationChannelViewModel> ProcessMessageAsync(IHubNotificationChannel iHubNotificationChannel)
        {
            var entity = _mapper.Map<NotificationChannelDomainEntity>(iHubNotificationChannel);
            if (!await _notificationChannelRepository.ExistsAsync(entity.Id))
            {
                return await AddNotificationChannelAsync(entity);
            }
            else
            {
                return await UpdateNotificationChannelAsync(entity);
            }
        }

        private async Task<NotificationChannelViewModel> AddNotificationChannelAsync(NotificationChannelDomainEntity entity)
        {
            var returnedDomainEntity = await _notificationChannelRepository.AddAsync(entity);
            return _mapper.Map<NotificationChannelViewModel>(returnedDomainEntity);
        }

        private async Task<NotificationChannelViewModel> UpdateNotificationChannelAsync(NotificationChannelDomainEntity entity)
        {
            var updatedDomainEntity = await _notificationChannelRepository.UpdateNotificationChannelAsync(entity);
            return _mapper.Map<NotificationChannelViewModel>(updatedDomainEntity);
        }
    }
}
