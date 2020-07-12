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
    public class BookController : BaseApiController
    {
        private readonly MyLibraryContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBookService _bookService;

        public BookController(MyLibraryContext dbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _bookService = new BookService(new BookUnitOfWork(_dbContext), _httpContextAccessor.HttpContext.User);
        }

        /// <summary>
        /// Used to add a book
        /// </summary>
        /// <param name="request">the request with the book information</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPost("")]
        public IActionResult AddBook(AddBookRequest request)
        {
            try
            {
                var response = _bookService.AddBook(request);

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
                s_logger.Error(ex, $"Unable to add book.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Used to get a specific book
        /// </summary>
        /// <param name="id">the id of the book to be retreived</param>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("{id}")]
        public IActionResult GetBook(int id)
        {
            try
            {
                var response = _bookService.GetBook(id);

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
                s_logger.Error(ex, $"Unable to retreive book with id [{id}].");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Used to get a books
        /// </summary>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("")]
        public IActionResult GetBooks()
        {
            try
            {
                var response = _bookService.GetBooks();

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
                s_logger.Error(ex, $"Unable to retreive books.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}