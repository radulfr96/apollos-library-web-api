﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ApollosLibrary.Application.Author.Commands.AddAuthorCommand;
using ApollosLibrary.Application.Author.Commands.DeleteAuthorCommand;
using ApollosLibrary.Application.Author.Commands.UpdateAuthorCommand;
using ApollosLibrary.Application.Author.Queries.GetAuthorQuery;
using ApollosLibrary.Application.Author.Queries.GetAuthorsQuery;
using ApollosLibrary.WebApi.Filters;
using ApollosLibrary.Application.Author.Queries.GetAuthorRecordQuery;

namespace ApollosLibrary.WebApi.Controllers
{
    /// <summary>
    /// Used to manage author records
    /// </summary>
    [ServiceFilter(typeof(SubscriptionFilterAttribute))]
    [Route("api/[controller]")]
    public class AuthorController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AuthorController(IMediator mediator, IConfiguration configuration) : base(configuration)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Used to add a new author
        /// </summary>
        /// <param name="command">The request with the author information</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPost("")]
        public async Task<AddAuthorCommandDto> AddAuthor([FromBody] AddAuthorCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Used to get authors
        /// </summary>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("")]
        public async Task<GetAuthorsQueryDto> GetAuthors()
        {
            return await _mediator.Send(new GetAuthorsQuery());
        }

        /// <summary>
        /// Used to get a specific author
        /// </summary>
        /// <param name="id">the id of the author to be retreived</param>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("{id}")]
        public async Task<GetAuthorQueryDto> GetAuthor([FromRoute] int id)
        {
            return await _mediator.Send(new GetAuthorQuery() { AuthorId = id });
        }

        /// <summary>
        /// Used to get a specific author record
        /// </summary>
        /// <param name="recordId">the id of the author record to be retreived</param>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("authorrecord/{recordId}")]
        public async Task<GetAuthorRecordQueryDto> GetAuthorRecord([FromRoute] int recordId)
        {
            return await _mediator.Send(new GetAuthorRecordQuery() { AuthorRecordId = recordId });
        }

        /// <summary>
        /// Used to update a author
        /// </summary>
        /// <param name="command">The information used to update the author</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPut("")]
        public async Task<UpdateAuthorCommandDto> UpdateGenre([FromBody] UpdateAuthorCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Used to remove a author from the system
        /// </summary>
        /// <param name="id">The id of the author to be deleted</param>
        /// <returns>Response that indicates the result</returns>
        [HttpDelete("{id}")]
        public async Task<DeleteAuthorCommandDto> DeleteAuthor([FromRoute] int id)
        {
            return await _mediator.Send(new DeleteAuthorCommand() { AuthorId = id });
        }
    }
}