using ApollosLibrary.Application.Library.Commands.CreateLibraryCommand;
using ApollosLibrary.Application.Library.Commands.DeleteLibraryEntryCommand;
using ApollosLibrary.Application.Library.Commands.UpdateLibraryEntryCommand;
using ApollosLibrary.Application.Library.Queries.GetLibraryEntriesQuery;
using ApollosLibrary.Application.Library.Queries.GetUserLibraryIdQuery;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ApollosLibrary.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        /// Used to update a library entry
        /// </summary>
        /// <param name="request">The information used to update the entry</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPatch("")]
        public async Task<UpdateLibraryEntryCommandDto> UpdateGenre([FromBody] UpdateLibraryEntryCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Used to remove a library entry from the system
        /// </summary>
        /// <param name="id">The id of the entry to be deleted</param>
        /// <returns>Response that indicates the result</returns>
        [HttpDelete("{id}")]
        public async Task<DeleteLibraryEntryCommandDto> DeleteBusiness([FromRoute] int id)
        {
            return await _mediator.Send(new DeleteLibraryEntryCommand() { LibraryEntryId = id });
        }
    }
}
