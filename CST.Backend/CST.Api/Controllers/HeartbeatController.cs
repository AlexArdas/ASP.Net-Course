using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CST.Api.Controllers
{
    [ApiController]
    [Route("api/heartbeat")]
    public class HeartbeatController : ControllerBase
    {
        /// <summary>
        /// Check that server works
        /// </summary>
        /// <returns>Current UTC time</returns>
        /// <response code="200">Returns current UTC time</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<DateTime> Get()
        { 
            return Ok(DateTime.UtcNow);
        }
    }
}
