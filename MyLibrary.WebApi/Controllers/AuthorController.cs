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
using MyLibrary.Application.Author.Commands.AddAuthorCommand;
using MyLibrary.Application.Author.Commands.DeleteAuthorCommand;
using MyLibrary.Application.Author.Commands.UpdateAuthorCommand;
using MyLibrary.Application.Author.Queries.GetAuthorQuery;

namespace MyLibrary.WebApi.Controllers
{
    [Authorize]
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
        /// <param name="request">The request with the author information</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPost("create")]
        public async Task<AddAuthorCommandDto> AddAuthor([FromBody] AddAuthorCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Used to get authors
        /// </summary>
        /// <returns>Response that indicates the result</returns>
        [HttpPost("authors")]
        public async Task<GetAuthorQueryDto> GetAuthors([FromBody] GetAuthorQuery query)
        {
            return await _mediator.Send(query);
        }

        /// <summary>
        /// Used to get a specific author
        /// </summary>
        /// <param name="id">the id of the author to be retreived</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPost("")]
        public async Task<GetAuthorQueryDto> GetAuthor([FromBody] GetAuthorQuery query)
        {
            return await _mediator.Send(query);
        }

        /// <summary>
        /// Used to update a author
        /// </summary>
        /// <param name="request">The information used to update the author</param>
        /// <returns>Response that indicates the result</returns>
        [HttpPatch("")]
        public async Task<UpdateAuthorCommandDto> UpdateGenre([FromBody] UpdateAuthorCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Used to remove a author from the system
        /// </summary>
        /// <param name="id">The id of the author to be deleted</param>
        /// <returns>Response that indicates the result</returns>
        [HttpDelete("")]
        public async Task<DeleteAuthorCommandDto> DeleteAuthor([FromBody] DeleteAuthorCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}