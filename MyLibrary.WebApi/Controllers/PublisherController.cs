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
using MyLibrary.Application.Publisher.Commands.AddPublisherCommand;
using MyLibrary.Application.Publisher.Commands.DeletePublisherCommand;
using MyLibrary.Application.Publisher.Commands.UpdatePublisherCommand;
using MyLibrary.Application.Publisher.Queries.GetPublisherQuery;
using MyLibrary.Application.Publisher.Queries.GetPublishersQuery;

namespace MyLibrary.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PublisherController : BaseApiController
    {
        private readonly IMediator _mediator;

        public PublisherController(IMediator mediator, IConfiguration configuration) : base(configuration)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Used to add a new publisher
        /// </summary>
        /// <param name="request">The request with the publisher information</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPost("create")]
        public async Task<AddPublisherCommandDto> AddPublisher([FromBody] AddPublisherCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Used to get publishers
        /// </summary>
        /// <returns>Response that indicates the result</returns>
        [HttpPost("publishers")]
        public async Task<GetPublishersQueryDto> GetPublishers([FromBody] GetPublishersQuery query)
        {
            return await _mediator.Send(query);
        }

        /// <summary>
        /// Used to get a specific publisher
        /// </summary>
        /// <param name="id">the id of the publisher to be retreived</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPost("")]
        public async Task<GetPublisherQueryDto> GetPublisher([FromBody] GetPublisherQuery query)
        {
            return await _mediator.Send(query);
        }

        /// <summary>
        /// Used to update a publisher
        /// </summary>
        /// <param name="request">The information used to update the publisher</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPatch("")]
        public async Task<UpdatePublisherCommandDto> UpdateGenre([FromBody] UpdatePublisherCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Used to remove a publisher from the system
        /// </summary>
        /// <param name="id">The id of the publisher to be deleted</param>
        /// <returns>Response that indicates the result</returns>
        [HttpDelete("")]
        public async Task<DeletePublisherCommandDto> DeletePublisher([FromBody] DeletePublisherCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}