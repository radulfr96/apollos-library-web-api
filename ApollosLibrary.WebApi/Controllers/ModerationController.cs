using ApollosLibrary.Application.Moderation.Commands.AddReportEntryCommand;
using ApollosLibrary.Application.Moderation.Queries.GetEntryReportQuery;
using ApollosLibrary.Application.Moderation.Queries.GetReportedEntryListQuery;
using ApollosLibrary.Application.Moderation.Queries.GetReportsOfEntriesByUserQuery;
using ApollosLibrary.Application.Moderation.Queries.GetUsersEntryReportsQuery;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

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
        public async Task<AddReportEntryCommandDto> AddEntryReport([FromBody] AddReportEntryCommand command)
        {
            return await _mediatr.Send(command);
        }

        /// <summary>
        /// Used to get reports for entries
        /// </summary>
        /// <returns>The reports</returns>
        [HttpGet("")]
        public async Task<GetReportedEntryListQueryDto> GetEntryReports()
        {
            return await _mediatr.Send(new GetReportedEntryListQuery());
        }


        /// <summary>
        /// Used to get report using the id provided for entries
        /// </summary>
        /// <param name="entryReportId">The id of the report to retreive</param>
        /// <returns>The report with the id provided</returns>
        [HttpGet("{entryReportId}")]
        public async Task<GetEntryReportQueryDto> GetEntryReport([FromRoute]int entryReportId)
        {
            return await _mediatr.Send(new GetEntryReportQuery()
            {
                ReportEntryId = entryReportId,
            });
        }

        /// <summary>
        /// Used to get reports of entries by a particular user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>The reports</returns>
        [HttpGet("reportedentriesbyuser/{userId}")]
        public async Task<GetReportsOfEntriesByUserQueryDto> GetReportsOfEntriesByUser([FromRoute] Guid userId)
        {
            return await _mediatr.Send(new GetReportsOfEntriesByUserQuery()
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
