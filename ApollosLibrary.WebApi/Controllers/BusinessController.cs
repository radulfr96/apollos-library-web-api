using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ApollosLibrary.Application.Business.Commands.AddBusinessCommand;
using ApollosLibrary.Application.Business.Commands.DeleteBusinessCommand;
using ApollosLibrary.Application.Business.Commands.UpdateBusinessCommand;
using ApollosLibrary.Application.Business.Queries.GetBusinessQuery;
using ApollosLibrary.Application.Business.Queries.GetBusinesssQuery;

namespace ApollosLibrary.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class BusinessController : BaseApiController
    {
        private readonly IMediator _mediator;

        public BusinessController(IMediator mediator, IConfiguration configuration) : base(configuration)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Used to add a new Business
        /// </summary>
        /// <param name="request">The request with the Business information</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPost("")]
        public async Task<AddBusinessCommandDto> AddBusiness([FromBody] AddBusinessCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Used to get Businesss
        /// </summary>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("")]
        public async Task<GetBusinesssQueryDto> GetBusinesss()
        {
            return await _mediator.Send(new GetBusinesssQuery());
        }

        /// <summary>
        /// Used to get a specific Business
        /// </summary>
        /// <param name="id">the id of the Business to be retreived</param>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("{id}")]
        public async Task<GetBusinessQueryDto> GetBusiness([FromRoute] int id)
        {
            return await _mediator.Send(new GetBusinessQuery() { BusinessId = id });
        }

        /// <summary>
        /// Used to update a Business
        /// </summary>
        /// <param name="request">The information used to update the Business</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPatch("")]
        public async Task<UpdateBusinessCommandDto> UpdateGenre([FromBody] UpdateBusinessCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Used to remove a Business from the system
        /// </summary>
        /// <param name="id">The id of the Business to be deleted</param>
        /// <returns>Response that indicates the result</returns>
        [HttpDelete("{id}")]
        public async Task<DeleteBusinessCommandDto> DeleteBusiness([FromRoute] int id)
        {
            return await _mediator.Send(new DeleteBusinessCommand() { PubisherId = id });
        }
    }
}