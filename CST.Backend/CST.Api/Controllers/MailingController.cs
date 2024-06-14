using System.Security.Claims;
using CST.Common.Services;
using CST.Common.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using CST.Common.Models.Context;
using AutoMapper;
using CST.Common.Exceptions;
using CST.Common.Models.Messages;
using Microsoft.AspNetCore.Authorization;
using CST.Common.Models.Enums;

namespace CST.Api.Controllers
{
    [ApiController]
    [Route("api/v1/mailings")]
    public class MailingController : ControllerBase
    {
        private readonly IMailingService _mailingService;
        private readonly INotificationChannelService _notificationChannelService;
        private readonly IMapper _mapper;

        public MailingController(IMailingService mailingService, INotificationChannelService notificationChannelService, IMapper mapper)
        {
            _mailingService = mailingService;
            _notificationChannelService = notificationChannelService;
            _mapper = mapper;
        }


        /// <summary>
        /// Get mailings' date range from the earliest to the newest mailing
        /// </summary>
        /// <returns>Date of the earliest and of the newest mailings</returns>
        /// <response code="200">Date of the earliest and of the newest mailings</response>
        [HttpGet("daterange")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<MailingDaterange>> GetDaterange()
        {
            return Ok(await _mailingService.GetMailingDaterangeAsync());
        }

        /// <summary>
        /// Get list of mailings filtered by date range, locations, subject, channel, author, statuses
        /// </summary>
        /// <param name="mailingFilterRequest">Filter parameters</param>
        /// <returns>Filtered list of mailings</returns>
        /// <response code="200">Filtered list of mailings</response>
        /// <response code="400">If the filter is null display error message</response>
        [HttpPost("filter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<MailingFilterResponse>>> FilterMailingsAsync(
            MailingFilterRequest mailingFilterRequest)
        {
            _ = mailingFilterRequest ?? throw new NullMailingFilterException(nameof(mailingFilterRequest));

            return Ok(await _mailingService.FilterMailingsAsync(mailingFilterRequest));
        }

        /// <summary>
        /// Test point. Create mailing (in case we don't have this mailing) or replace it with an updated mailing (removes original with the same IHubId permanently)
        /// </summary>
        /// <param name="mailing">Mailing to create/update</param>
        /// <returns>Created/updated mailing</returns>
        /// <response code="200">Created/updated mailing</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<MailingViewModel>> ProcessMessage([FromBody] IHubMailing mailing)
        {
            return Ok(await _mailingService.ProcessMessageAsync(mailing));
        }

        /// <summary>
        /// Get mailing description.
        /// </summary>
        /// <returns>Mailing description model</returns>
        /// <response code="200">Mailing description</response>
        /// <response code="404">If mailing id was not found display error message</response>
        [HttpGet("{mailingId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MailingDescriptionResponse>> GetMailingDescription(Guid mailingId)
        {
            return Ok(await _mailingService.GetMailingDescriptionAsync(mailingId));
        }

        /// <summary>
        /// Cancel mailing by mailingId
        /// </summary>
        /// <param name="mailingId">Mailing Id</param>
        /// <response code="200">Mailing canceled</response>
        /// <response code="400">Mailing is not canceled</response>
        /// <response code="500">Mailing is not canceled at IHub</response>
        [HttpPut("{mailingId}/cancel")]
        [Authorize(Roles = $"{nameof(RoleNames.CstHubAdmin)}, {nameof(RoleNames.CstMccManager)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CancelMailingByIdAsync(Guid mailingId)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            await _mailingService.CancelMailingAsync(mailingId, userEmail);

            return Ok();
        }
    }
}
