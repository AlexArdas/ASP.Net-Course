using CST.Common.Services;
using CST.Common.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CST.Common.Models.Pagination;

namespace CST.Api.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get current user
        /// </summary>
        /// <returns>Current user with roles</returns>
        /// <response code="200">Current user with roles</response>
        /// <response code="404">If user was not found</response>
        [HttpGet("current")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponse>> GetCurrentUserAsync()
        {
            var currentUserEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (currentUserEmail is null)
            {
                return NotFound($"User email was not defined.");
            }
            var currentUserResponseModel = await _userService
                .GetUserInfoByEmailAsync(currentUserEmail);

            return Ok(currentUserResponseModel);
        }

        /// <summary>
        /// Get users by part of full name
        /// </summary>
        /// <returns>List of users</returns>
        /// <response code="200">List of users</response>
        /// <response code="400">If request contains null or empty string</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaginatedList<UserBriefResponse>>> FilterUsersByFullNameAsync([FromQuery]string searchOption, [FromQuery]PaginationParameters paginationParameters)
        {
            var users = await _userService.FilterUsersByFullNameAsync(searchOption, paginationParameters);
            return Ok(users);
        }
    }
}
