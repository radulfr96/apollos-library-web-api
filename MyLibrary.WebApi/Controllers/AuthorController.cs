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
    [Authorize]
    [Route("api/[controller]")]
    public class AuthorController : BaseApiController
    {
        private readonly MyLibraryContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthorService _authorService;

        public AuthorController(MyLibraryContext dbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration)

        {
            _dbContext = dbContext;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _authorService = new AuthorService(new AuthorUnitOfWork(_dbContext), _httpContextAccessor.HttpContext.User);
        }

        /// <summary>
        /// Used to add a new author
        /// </summary>
        /// <param name="request">The request with the author information</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPost("")]
        public IActionResult AddAuthor([FromBody] AddAuthorRequest request)
        {
            try
            {
                var response = _authorService.AddAuthor(request);

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
                s_logger.Error(ex, $"Unable to add author.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Used to get authors
        /// </summary>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("")]
        public IActionResult GetAuthors()
        {
            try
            {
                var response = _authorService.GetAuthors();

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
                s_logger.Error(ex, "Unable to retreive authors.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Used to get a specific author
        /// </summary>
        /// <param name="id">the id of the author to be retreived</param>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("{id}")]
        public IActionResult GetAuthor([FromRoute] int id)
        {
            try
            {
                var response = _authorService.GetAuthor(id);

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
                s_logger.Error(ex, $"Unable to retreive author with id {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Used to update a author
        /// </summary>
        /// <param name="request">The information used to update the author</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPatch("")]
        public IActionResult UpdateGenre([FromBody] UpdateAuthorRequest request)
        {
            try
            {
                var response = _authorService.UpdateAuthor(request);

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
                s_logger.Error(ex, $"Unable to update author.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Used to remove a author from the system
        /// </summary>
        /// <param name="id">The id of the author to be deleted</param>
        /// <returns>Response that indicates the result</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteAuthor([FromRoute] int id)
        {
            if (!UserIsAdmin())
                return Forbid();

            try
            {
                var response = _authorService.DeleteAuthor(id);

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
                s_logger.Error(ex, $"Unable to delete author.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}