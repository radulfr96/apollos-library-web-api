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
using ApollosLibrary.WebApi.Filters;
using ApollosLibrary.Domain.Enums;
using ApollosLibrary.Application.Business.Queries.GetBusinessRecordQuery;

namespace ApollosLibrary.WebApi.Controllers
{
    /// <summary>
    /// Used to manage business entries
    /// </summary>
    [ServiceFilter(typeof(SubscriptionFilterAttribute))]
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
        /// <param name="command">The request with the Business information</param>
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
        /// Used to get bookshops
        /// </summary>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("bookshops")]
        public async Task<GetBusinesssQueryDto> GetBookshops()
        {
            return await _mediator.Send(new GetBusinesssQuery()
            {
                BusinessType = BusinessTypeEnum.Bookshop,
            });
        }

        /// <summary>
        /// Used to get publishers
        /// </summary>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("publishers")]
        public async Task<GetBusinesssQueryDto> GetPublishers()
        {
            return await _mediator.Send(new GetBusinesssQuery()
            {
                BusinessType = BusinessTypeEnum.Publisher,
            });
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
        /// Used to get a specific business record
        /// </summary>
        /// <param name="recordId">the id of the business record to be retreived</param>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("businessrecord/{recordId}")]
        public async Task<GetBusinessRecordQueryDto> GetBusinessRecord([FromRoute] int recordId)
        {
            return await _mediator.Send(new GetBusinessRecordQuery() { BusinessRecordId = recordId });
        }

        /// <summary>
        /// Used to update a Business
        /// </summary>
        /// <param name="command">The information used to update the Business</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPut("")]
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
            return await _mediator.Send(new DeleteBusinessCommand() { BusinessId = id });
        }
    }
}