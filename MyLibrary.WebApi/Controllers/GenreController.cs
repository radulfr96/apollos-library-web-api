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
using MyLibrary.Application.Genre.Commands.AddGenreCommand;
using MyLibrary.Application.Genre.Commands.DeleteGenreCommand;
using MyLibrary.Application.Genre.Commands.UpdateGenreCommand;
using MyLibrary.Application.Genre.Queries.GetGenreQuery;
using MyLibrary.Application.Genre.Queries.GetGenresQuery;

namespace MyLibrary.WebApi.Controllers
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
        [HttpPost("create")]
        public async Task<AddGenreCommandDto> AddGenre([FromBody] AddGenreCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Used to get genres
        /// </summary>
        /// <returns>Response that indicates the result</returns>
        [HttpPost("genres")]
        public async Task<GetGenresQueryDto> GetGenres([FromBody] GetGenresQuery query)
        {
            return await _mediator.Send(query);
        }

        /// <summary>
        /// Used to get a specific genre
        /// </summary>
        /// <param name="id">the id of the genre to be retreived</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPost("")]
        public async Task<GetGenreQueryDto> GetGenre([FromBody] GetGenreQuery query)
        {
            return await _mediator.Send(query);
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