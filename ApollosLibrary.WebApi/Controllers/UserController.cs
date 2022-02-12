using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ApollosLibrary.Application.User.Commands.UpdatePasswordCommand;
using ApollosLibrary.Application.User.Commands.UpdateSelfUserCommand;
using ApollosLibrary.Application.User.Queries.CheckMyUsernameUnique;
using ApollosLibrary.Application.User.Queries.GetUserQuery;
using ApollosLibrary.Application.User.Queries.GetUsersQuery;
using System.Threading.Tasks;
using System;

namespace ApollosLibrary.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : BaseApiController
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator, IConfiguration configuration) : base(configuration)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Used to get users
        /// </summary>
        /// <returns>Response that indicates the result</returns>
        [HttpPost("users")]
        public async Task<GetUsersQueryDto> GetUsers([FromBody] GetUsersQuery query)
        {
            return await _mediator.Send(query);
        }

        /// <summary>
        /// Used to get a specific user
        /// </summary>
        /// <param name="id">the id of the user to be retreived</param>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("{id}")]
        public async Task<GetUserQueryDto> GetUser([FromRoute] Guid id)
        {
            return await _mediator.Send(new GetUserQuery() { UserId = id });
        }

        /// <summary>
        /// Used by the user to update themself
        /// </summary>
        /// <param name="command">the command with the user data</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPatch("selfupdate")]
        public async Task<UpdateSelfUserCommandDto> UpdateSelf([FromBody] UpdateSelfUserCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Used to get a specific user
        /// </summary>
        /// <param name="username">the username to be checked</param>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("checkselfusername/{username}")]
        public async Task<CheckMyUsernameUniqueQueryDto> CheckUsernameIsUniqueSelf([FromRoute] string username)
        {
            return await _mediator.Send(new CheckMyUsernameUniqueQuery()
            {
                Username = username,
            });
        }

        /// <summary>
        /// Used by user to update their password
        /// </summary>
        /// <param name="command">The request to update the password</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPatch("password")]
        public async Task<UpdatePasswordCommandDto> UpdatePassword([FromRoute] UpdatePasswordCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
