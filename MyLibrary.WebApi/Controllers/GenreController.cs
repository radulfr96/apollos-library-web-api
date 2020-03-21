using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyLibrary.Common.Requests;
using MyLibrary.Data.Model;
using MyLibrary.Services;
using MyLibrary.Services.Contracts;
using MyLibrary.UnitOfWork;

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
        private readonly MyLibraryContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGenreService _genreService;

        public GenreController(MyLibraryContext dbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _genreService = new GenreService(new GenreUnitOfWork(_dbContext), _httpContextAccessor.HttpContext.User);

        }

        /// <summary>
        /// Used to add a new genre
        /// </summary>
        /// <param name="request">The request with the genre information</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPost("")]
        public IActionResult AddGenre([FromBody] AddGenreRequest request)
        {
            if (!UserIsAdmin())
                return Forbid();

            try
            {
                var response = _genreService.AddGenre(request);

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
                s_logger.Error(ex, $"Unable to add genre.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Used to get genres
        /// </summary>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("")]
        public IActionResult GetGenres()
        {
            try
            {
                var response = _genreService.GetGenres();

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
                s_logger.Error(ex, "Unable to retreive genres.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Used to get a specific genre
        /// </summary>
        /// <param name="id">the id of the genre to be retreived</param>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("{id}")]
        public IActionResult GetGenre([FromRoute] int id)
        {
            if (!UserIsAdmin())
                return Forbid();

            try
            {
                var response = _genreService.GetGenre(id);

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
                s_logger.Error(ex, $"Unable to retreive genre with id {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Used to update a genre
        /// </summary>
        /// <param name="request">The information used to update the genre</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPatch("")]
        public IActionResult UpdateGenre([FromBody] UpdateGenreRequest request)
        {
            if (!UserIsAdmin())
                return Forbid();

            try
            {
                var response = _genreService.UpdateGenre(request);

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
                s_logger.Error(ex, $"Unable to update genre.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Used to remove a genre from the system
        /// </summary>
        /// <param name="id">The id of the genre to be deleted</param>
        /// <returns>Response that indicates the result</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteGenre([FromRoute] int id)
        {
            if (!UserIsAdmin())
                return Forbid();

            try
            {
                var response = _genreService.DeleteGenre(id);

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
                s_logger.Error(ex, $"Unable to delete genre.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}