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
using ApollosLibrary.Application.Genre.Commands.AddGenreCommand;
using ApollosLibrary.Application.Genre.Commands.DeleteGenreCommand;
using ApollosLibrary.Application.Genre.Commands.UpdateGenreCommand;
using ApollosLibrary.Application.Genre.Queries.GetGenreQuery;
using ApollosLibrary.Application.Genre.Queries.GetGenresQuery;

namespace ApollosLibrary.WebApi.Controllers
{
    /// <summary>
    /// Controller used as an endpoint for genre functions
    /// </summary>
    [Authorize]
    [Route("api/genre")]
    [ApiController]
    public class GenreController : BaseApiController
    {
        private readonly IMediator _mediator;

        public GenreController(IMediator mediator, IConfiguration configuration) : base(configuration)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Used to add a new genre
        /// </summary>
        /// <param name="request">The request with the genre information</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPost("")]
        public async Task<AddGenreCommandDto> AddGenre([FromBody] AddGenreCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Used to get genres
        /// </summary>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("")]
        public async Task<GetGenresQueryDto> GetGenres()
        {
            return await _mediator.Send(new GetGenresQuery());
        }

        /// <summary>
        /// Used to get a specific genre
        /// </summary>
        /// <param name="id">the id of the genre to be retreived</param>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("{id}")]
        public async Task<GetGenreQueryDto> GetGenre([FromRoute] int id)
        {
            return await _mediator.Send(new GetGenreQuery() { GenreId = id });
        }

        /// <summary>
        /// Used to update a genre
        /// </summary>
        /// <param name="request">The information used to update the genre</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPatch("")]
        public async Task<UpdateGenreCommandDto> UpdateGenre([FromBody] UpdateGenreCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Used to remove a genre from the system
        /// </summary>
        /// <param name="id">The id of the genre to be deleted</param>
        /// <returns>Response that indicates the result</returns>
        [HttpDelete("{id}")]
        public async Task<DeleteGenreCommandDto> DeleteGenre([FromBody] DeleteGenreCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}