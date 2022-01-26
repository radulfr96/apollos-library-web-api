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
using ApollosLibrary.Application.Book.Commands.AddBookCommand;
using ApollosLibrary.Application.Book.Commands.UpdateBookCommand;
using ApollosLibrary.Application.Book.Queries.GetBookQuery;
using ApollosLibrary.Application.Book.Queries.GetBooksQuery;

namespace ApollosLibrary.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class BookController : BaseApiController
    {
        private readonly IMediator _mediator;

        public BookController(IMediator mediator, IConfiguration configuration) : base(configuration)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Used to add a book
        /// </summary>
        /// <param name="request">the request with the book information</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPost("create")]
        public async Task<AddBookCommandDto> AddBook([FromBody] AddBookCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Used to get a specific book
        /// </summary>
        /// <param name="id">the id of the book to be retreived</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPost("")]
        public async Task<GetBookQueryDto> GetBook([FromBody] GetBookQuery query)
        {
            return await _mediator.Send(query);
        }

        /// <summary>
        /// Used to get a books
        /// </summary>
        /// <returns>Response that indicates the result</returns>
        [HttpPost("books")]
        public async Task<GetBooksQueryDto> GetBooks([FromBody] GetBooksQuery query)
        {
            return await _mediator.Send(query);
        }

        /// <summary>
        /// Used to update a book
        /// </summary>
        /// <param name="request">the request with the book information</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPatch("")]
        public async Task<UpdateBookCommandDto> UpdateBook(UpdateBookCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}