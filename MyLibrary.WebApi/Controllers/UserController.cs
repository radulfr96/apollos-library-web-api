using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyLibrary.Common.Requests;
using MyLibrary.Common.Responses;
using MyLibrary.Contracts.UnitOfWork;
using MyLibrary.Data.Model;
using MyLibrary.Services;
using MyLibrary.Services.Contracts;
using MyLibrary.UnitOfWork;
using NLog;

namespace MyLibrary.WebApi.Controllers
{
    /// <summary>
    /// Controller used as an endpoint for user functions
    /// </summary>
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UserController : BaseApiController
    {
        private readonly MyLibraryContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        public UserController(MyLibraryContext dbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base (configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _userService = new UserService(new UserUnitOfWork(_dbContext), _configuration);
        }

        [AllowAnonymous]
        [HttpPost("")]
        public IActionResult RegisterUser(RegisterUserRequest request)
        {
            try
            {
                var response = _userService.Register(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return Ok(response);
                    case HttpStatusCode.BadRequest:
                        return BadRequest(BuildBadRequestMessage(response));
                    case HttpStatusCode.InternalServerError:
                        return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to login user.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Used to update the users username
        /// </summary>
        /// <param name="request">The update information</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPatch("")]
        public IActionResult UpdateUsername(UpdateUsernameRequest request)
        {
            try
            {
                var userIdString = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value;

                int? userId = int.Parse(userIdString);

                if (!userId.HasValue)
                {
                    return Forbid();
                }

                var response = _userService.UpdateUsername(request, userId.Value);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                          return Ok(response);
                    case HttpStatusCode.BadRequest:
                        return BadRequest(BuildBadRequestMessage(response));
                    case HttpStatusCode.InternalServerError:
                        return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to login user.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Used to get all users
        /// </summary>
        /// <returns>The get all users response</returns>
        [HttpGet("")]
        public IActionResult GetUsers()
        {
            if (!IsAdmin())
                return new StatusCodeResult(StatusCodes.Status403Forbidden);

            try
            {
                var response = _userService.GetUsers();

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return Ok(response);
                    case HttpStatusCode.BadRequest:
                        return BadRequest(BuildBadRequestMessage(response));
                    case HttpStatusCode.NotFound:
                        return NotFound();
                    case HttpStatusCode.InternalServerError:
                        return StatusCode(StatusCodes.Status500InternalServerError);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to retreive users.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Used to check if username is taken
        /// </summary>
        /// <returns>The get result</returns>
        [AllowAnonymous]
        [HttpGet("{username}")]
        public IActionResult CheckUsernameTaken(string username)
        {
            try
            {
                var response = _userService.UsernameCheck(username);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return Ok(response);
                    case HttpStatusCode.BadRequest:
                        return BadRequest(BuildBadRequestMessage(response));
                    case HttpStatusCode.NotFound:
                        return NotFound();
                    case HttpStatusCode.InternalServerError:
                        return StatusCode(StatusCodes.Status500InternalServerError);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to retreive users.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Used to login a user
        /// </summary>
        /// <param name="request">The request with the users login information</param>
        /// <returns>The response containing the users token</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = _userService.Login(request);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return Ok(response);
                    case HttpStatusCode.BadRequest:
                        return BadRequest(BuildBadRequestMessage(response));
                    case HttpStatusCode.NotFound:
                        return NotFound();
                    case HttpStatusCode.InternalServerError:
                        return StatusCode(StatusCodes.Status500InternalServerError);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to login user.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Used to get information about the calling user
        /// </summary>
        /// <returns>The users information</returns>
        [HttpGet("userinfo")]
        public IActionResult GetUserInfo()
        {
            var response = new GetUserInfoResponse();

            try
            {
                var user = _httpContextAccessor.HttpContext.User;
                response.Username = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
                response.Roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

                var userID = user.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(userID))
                {
                    if (!int.TryParse(userID, out int userIdInt))
                    {
                        s_logger.Error("Unable to get user id from claims");
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }

                    response.UserID = userIdInt;
                }

                var dateString = user.Claims.Where(c => c.Type == "JoinDate").Select(c => c.Value).FirstOrDefault();

                if (!string.IsNullOrEmpty(dateString))
                {
                    response.JoinDate = DateTime.Parse(dateString);
                }

                response.StatusCode = HttpStatusCode.OK;
                return Ok(response);
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to login user.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}