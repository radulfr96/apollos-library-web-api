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
using ApollosLibrary.Application.Book.Commands.DeleteBookCommand;

namespace ApollosLibrary.WebApi.Controllers
{
    /// <summary>
    /// Used to manage books in system
    /// </summary>
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
        /// <param name="command">the request with the book information</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPost("")]
        public async Task<AddBookCommandDto> AddBook([FromBody] AddBookCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Used to get a specific book
        /// </summary>
        /// <param name="id">the id of the book to be retreived</param>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("{id}")]
        public async Task<GetBookQueryDto> GetBook([FromRoute] int id)
        {
            return await _mediator.Send(new GetBookQuery()
            {
                BookId = id,
            });
        }

        /// <summary>
        /// Used to get a books
        /// </summary>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("")]
        public async Task<GetBooksQueryDto> GetBooks()
        {
            return await _mediator.Send(new GetBooksQuery());
        }

        /// <summary>
        /// Used to update a book
        /// </summary>
        /// <param name="command">the request with the book information</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPut("")]
        public async Task<UpdateBookCommandDto> UpdateBook(UpdateBookCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Used to delete a book
        /// </summary>
        /// <param name="bookId">the book id</param>
        /// <returns>Response that indicates the result</returns>
        [HttpDelete("{bookId}")]
        public async Task<DeleteBookCommandDto> UpdateBook([FromRoute]int bookId)
        {
            return await _mediator.Send(new DeleteBookCommand()
            {
                BookId = bookId,
            });
        }
    }
}