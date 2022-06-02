using ApollosLibrary.Application.Library.Commands.AddLibraryEntryCommand;
using ApollosLibrary.Application.Library.Commands.CreateLibraryCommand;
using ApollosLibrary.Application.Library.Commands.DeleteLibraryEntryCommand;
using ApollosLibrary.Application.Library.Commands.UpdateLibraryEntryCommand;
using ApollosLibrary.Application.Library.Queries.GetBooksInLibraryQuery;
using ApollosLibrary.Application.Library.Queries.GetLibraryEntriesQuery;
using ApollosLibrary.Application.Library.Queries.GetLibraryEntryQuery;
using ApollosLibrary.Application.Library.Queries.GetUserLibraryIdQuery;
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
    /// Used to manage a users library
    /// </summary>
    [ServiceFilter(typeof(SubscriptionFilterAttribute))]
    [Route("api/[controller]")]
    public class LibraryController : BaseApiController
    {
        private readonly IMediator _mediator;

        public LibraryController(IMediator mediator, IConfiguration configuration) : base(configuration)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Used to create a new library
        /// </summary>
        /// <returns>Response that indicates the result</returns>
        [HttpPost("")]
        public async Task<CreateLibraryCommandDto> CreateLibrary()
        {
            return await _mediator.Send(new CreateLibraryCommand());
        }

        /// <summary>
        /// Used to create a library entry
        /// </summary>
        /// <returns>Response that indicates the result</returns>
        [HttpPost("addentry")]
        public async Task<AddLibraryEntryCommandDto> AddLibraryEntry([FromBody] AddLibraryEntryCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Used to get a users library id
        /// </summary>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("userlibraryid")]
        public async Task<GetUserLibraryIdQueryDto> GetUserLibraryId()
        {
            return await _mediator.Send(new GetUserLibraryIdQuery());
        }

        /// <summary>
        /// Used to get a users library entries
        /// </summary>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("")]
        public async Task<GetLibraryEntriesQueryDto> GetLibraryEntries()
        {
            return await _mediator.Send(new GetLibraryEntriesQuery());
        }

        /// <summary>
        /// Used to get a users library books
        /// </summary>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("books")]
        public async Task<GetBooksLibraryInQueryDto> GetLibraryBooks()
        {
            return await _mediator.Send(new GetBooksInLibraryQuery());
        }

        /// <summary>
        /// Used to get a library entry
        /// </summary>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("entry/{id}")]
        public async Task<GetLibraryEntryQueryDto> GetLibraryEntry([FromRoute] int id)
        {
            return await _mediator.Send(new GetLibraryEntryQuery() { EntryId = id });
        }

        /// <summary>
        /// Used to update a library entry
        /// </summary>
        /// <param name="command">The information used to update the entry</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPut("")]
        public async Task<UpdateLibraryEntryCommandDto> UpdateLibraryEntry([FromBody] UpdateLibraryEntryCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Used to remove a library entry from the system
        /// </summary>
        /// <param name="id">The id of the entry to be deleted</param>
        /// <returns>Response that indicates the result</returns>
        [HttpDelete("{id}")]
        public async Task<DeleteLibraryEntryCommandDto> DeleteLibraryEntry([FromRoute] int id)
        {
            return await _mediator.Send(new DeleteLibraryEntryCommand() { LibraryEntryId = id });
        }
    }
}
