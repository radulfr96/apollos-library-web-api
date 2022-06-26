using ApollosLibrary.Application.Moderation.Commands.AddEntryReportCommand;
using ApollosLibrary.Application.Moderation.Queries.GetEntryReportQuery;
using ApollosLibrary.Application.Moderation.Queries.GetEntryReportsQuery;
using ApollosLibrary.Application.Moderation.Queries.GetEntryReportsByEntryUserQuery;
using ApollosLibrary.Application.Moderation.Queries.GetUsersEntryReportsQuery;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using ApollosLibrary.Application.Moderation.Commands.UpdateEntryReportCommand;
using ApollosLibrary.Application.Moderation.Queries.GetUsersQuery;

namespace ApollosLibrary.WebApi.Controllers
{
    /// <summary>
    /// Used to manage moderation
    /// </summary>
    [Route("api/[controller]")]
    public class ModerationController : BaseApiController
    {
        private readonly IMediator _mediatr;

        public ModerationController(IConfiguration config, IMediator mediatr) : base(config)
        {
            _mediatr = mediatr;
        }

        /// <summary>
        /// Used to add a new entry report
        /// </summary>
        /// <param name="command">The report details</param>
        /// <returns>Response indicating the result</returns>
        [HttpPost("")]
        public async Task<AddEntryReportCommandDto> AddEntryReport([FromBody] AddEntryReportCommand command)
        {
            return await _mediatr.Send(command);
        }

        /// <summary>
        /// Used to update an entry report
        /// </summary>
        /// <param name="command">The report details</param>
        /// <returns>Response indicating the result</returns>
        [HttpPut("")]
        public async Task<UpdateEntryReportCommandDto> UpdateEntryReport([FromBody] UpdateEntryReportCommand command)
        {
            return await _mediatr.Send(command);
        }

        /// <summary>
        /// Used to get all entry reports
        /// </summary>
        /// <returns>The reports</returns>
        [HttpGet("")]
        public async Task<GetEntryReportsQueryDto> GetEntryReports()
        {
            return await _mediatr.Send(new GetEntryReportsQuery());
        }

        /// <summary>
        /// Used to get all users with either reports or or reported entries
        /// </summary>
        /// <returns>The reports</returns>
        [HttpGet("users")]
        public async Task<GetUsersQueryDto> GetUsers()
        {
            return await _mediatr.Send(new GetUsersQuery());
        }

        /// <summary>
        /// Used to get report using the id provided
        /// </summary>
        /// <param name="entryReportId">The id of the report to retreive</param>
        /// <returns>The report with the id provided</returns>
        [HttpGet("{entryReportId}")]
        public async Task<GetEntryReportQueryDto> GetEntryReport([FromRoute]int entryReportId)
        {
            return await _mediatr.Send(new GetEntryReportQuery()
            {
                EntryReportId = entryReportId,
            });
        }

        /// <summary>
        /// Used to get reports of entries by entry user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>The reports</returns>
        [HttpGet("reportedentriesbyuser/{userId}")]
        public async Task<GetEntryReportsByEntryUserQueryDto> GetEntryReportsByEntryUser([FromRoute] Guid userId)
        {
            return await _mediatr.Send(new GetEntryReportsByEntryUserQuery()
            {
                UserId = userId,
            });
        }

        /// <summary>
        /// Used to get reports by a particular user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>The reports</returns>
        [HttpGet("reportsbyuser/{userId}")]
        public async Task<GetUsersEntryReportsQueryDto> GetUsersEntryReportsQuery([FromRoute] Guid userId)
        {
            return await _mediatr.Send(new GetUsersEntryReportsQuery()
            {
                UserId = userId,
            });
        }
    }
}
