using CST.Common.Models.DTO;
using CST.Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace CST.Api.Controllers
{
    [ApiController]
    [Route("api/v1/locations")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;
        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        /// <summary>
        /// Get all locations
        /// </summary>
        /// <returns>List of locations</returns>
        /// <response code="200">List of locations</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<LocationViewModel>>> GetLocationsAsync()
        {
            return Ok(await _locationService.GetLocationsAsync());
        }
    }
}
