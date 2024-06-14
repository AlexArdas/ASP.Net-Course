using CST.Common.Models.DTO;
using CST.Common.Models.Messages;

namespace CST.Common.Services
{
    public interface INotificationChannelService
    {
        Task<NotificationChannelViewModel> ProcessMessageAsync(IHubNotificationChannel viewModel);
    }
}
