﻿using ApollosLibrary.Application.Series.Commands.AddSeriesCommand;
using ApollosLibrary.Application.Series.Commands.DeleteSeriesCommand;
using ApollosLibrary.Application.Series.Commands.UpdateSeriesCommand;
using ApollosLibrary.Application.Series.Queries.GetMultiSeriesQuery;
using ApollosLibrary.Application.Series.Queries.GetSeriesQuery;
using ApollosLibrary.Application.Series.Queries.GetSeriesRecordQuery;
using ApollosLibrary.WebApi.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ApollosLibrary.WebApi.Controllers
{
    /// <summary>
    /// Used to manage the series entries in the system
    /// </summary>
    [ServiceFilter(typeof(SubscriptionFilterAttribute))]
    [Route("api/[controller]")]
    public class SeriesController : BaseApiController
    {
        private readonly IMediator _mediator;

        public SeriesController(IMediator mediator, IConfiguration configuration) : base(configuration)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Used to add a new series
        /// </summary>
        /// <param name="command">The request with the series information</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPost("")]
        public async Task<AddSeriesCommandDto> AddSeries([FromBody] AddSeriesCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Used to get series
        /// </summary>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("")]
        public async Task<GetMultiSeriesQueryDto> GetSeries()
        {
            return await _mediator.Send(new GetMultiSeriesQuery());
        }

        /// <summary>
        /// Used to get a specific series
        /// </summary>
        /// <param name="id">the id of the series to be retreived</param>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("{id}")]
        public async Task<GetSeriesQueryDto> GetSeries([FromRoute] int id)
        {
            return await _mediator.Send(new GetSeriesQuery() { SeriesId = id });
        }

        /// <summary>
        /// Used to get a specific series record
        /// </summary>
        /// <param name="recordId">the id of the series record to be retreived</param>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("seriesrecord/{recordId}")]
        public async Task<GetSeriesRecordQueryDto> GetSeriesRecord([FromRoute] int recordId)
        {
            return await _mediator.Send(new GetSeriesRecordQuery() { SeriesRecordId = recordId });
        }

        /// <summary>
        /// Used to update a series
        /// </summary>
        /// <param name="command">The information used to update the series</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPut("")]
        public async Task<UpdateSeriesCommandDto> UpdateGenre([FromBody] UpdateSeriesCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Used to remove a series from the system
        /// </summary>
        /// <param name="id">The id of the series to be deleted</param>
        /// <returns>Response that indicates the result</returns>
        [HttpDelete("{id}")]
        public async Task<DeleteSeriesCommandDto> DeleteGenre([FromRoute] int id)
        {
            return await _mediator.Send(new DeleteSeriesCommand() { SeriesId = id });
        }
    }
}
