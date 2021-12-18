using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyLibrary.Application.User.Queries.GetUserQuery;
using MyLibrary.Application.User.Queries.GetUsersQuery;
using System.Threading.Tasks;

namespace MyLibrary.WebApi.Controllers
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
        [HttpPost("")]
        public async Task<GetUserQueryDto> GetUser([FromBody] GetUserQuery query)
        {
            return await _mediator.Send(query);
        }
    }
}
