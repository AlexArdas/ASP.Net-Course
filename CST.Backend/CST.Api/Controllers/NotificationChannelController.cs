using CST.Common.Models.DTO;
using CST.Common.Models.Messages;
using CST.Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace CST.Api.Controllers
{
    [ApiController]
    [Route("api/v1/notificationchannel")]
    public class NotificationChannelController : Controller
    {
        private readonly INotificationChannelService _notificationChannelService;
        
        public NotificationChannelController(INotificationChannelService notificationChannelService)
        {
            _notificationChannelService = notificationChannelService;
        }

        /// <summary>
        /// Process the notification channel status update message
        /// </summary>
        /// <param name="channel">Notification channel model to create/update</param> 
        /// <returns>Processed notification channel</returns>
        /// <response code="200">Processed notification channel</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<NotificationChannelViewModel>> ProcessMessage([FromBody] IHubNotificationChannel channel)
        {
            var result = await _notificationChannelService.ProcessMessageAsync(channel);
            return Ok(result);
        }
    }
}
