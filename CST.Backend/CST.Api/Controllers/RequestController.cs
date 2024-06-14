using CST.Common.Models.DTO;
using CST.Common.Models.Enums;
using CST.Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace CST.Api.Controllers
{
    [ApiController]
    [Route("api/v1/requests")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        /// <summary>
        /// Get all requests.
        /// </summary>
        /// <returns>List of requests.</returns>
        /// <response code="200">List of requests.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<RequestResponse>>> GetAll()
        {
            return Ok(await _requestService.GetRequestsAsync());
        }

        /// <summary>
        /// Get all request messages.
        /// </summary>
        /// <returns>List of request messages.</returns>
        /// <response code="200">List of requests messages.</response>
        /// <response code="404">If request not found display error message</response>
        [HttpGet("{requestId}/messages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RequestMessagesWithFormResponse>> GetRequestMessages(Guid requestId)
        {
            return Ok(await _requestService.GetRequestMessagesWithFormAsync(requestId));
        }

        /// <summary>
        /// Assign request with requestId on user with userId.
        /// </summary>
        /// <returns>Assigned request</returns>
        /// <response code="200">Assignation accepted</response>
        /// <response code="403">You don't have permission for this operation</response>
        /// <response code="404">UserId or RequestId wasn't found</response>
        [HttpPut("{requestId}/assign")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AssignRequest(Guid requestId, Guid userId)
        {
            await _requestService.AssignRequestAsync(requestId, userId);
            return Ok();
        }

        /// <summary>
        /// Close request.
        /// </summary>
        /// <returns>Success response.</returns>
        /// <response code="200">Success response.</response>
        /// <response code="403">You don't have permission for this operation</response>
        /// <response code="404">If request not found display error message</response>
        [HttpPost("{requestId}/close")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> CloseRequest(Guid requestId)
        {
            await _requestService.UpdateRequestStatusAsync(requestId, RequestStatus.Closed);
            return Ok();
        }

        /// <summary>
        /// Reopen request.
        /// </summary>
        /// <returns>Success response.</returns>
        /// <response code="200">Success response.</response>
        /// <response code="403">You don't have permission for this operation</response>
        /// <response code="404">If request not found display error message</response>
        [HttpPost("{requestId}/reopen")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ReopenRequest(Guid requestId)
        {
            await _requestService.UpdateRequestStatusAsync(requestId, RequestStatus.InProgress);
            return Ok();
        }

        /// <summary>
        /// Get count of unread or updated requests.
        /// </summary>
        /// <returns>Count of unread or updated requests.</returns>
        /// <response code="200">Count of unread or updated requests.</response>
        [HttpGet("unread/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetUnreadRequestsCount()
        {
            return Ok(await _requestService.GetUnreadRequestsCountAsync());
        }
    }
}