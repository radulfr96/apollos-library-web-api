using ApollosLibrary.Application.Author.Commands.AddAuthorCommand;
using ApollosLibrary.Application.Dashboard.Queries.GetUserBudgetProgressQuery;
using ApollosLibrary.WebApi.Filters;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ApollosLibrary.WebApi.Controllers
{
    /// <summary>
    /// Used to 
    /// </summary>
    [ServiceFilter(typeof(SubscriptionFilterAttribute))]
    [Route("api/[controller]")]
    public class DashboardController : BaseApiController
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator, IConfiguration configuration) : base(configuration)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Used to get the users budget report for the year provided
        /// </summary>
        /// <param name="year">The year of the report information</param>
        /// <returns>Response with the report data</returns>
        [HttpGet("budgetreport/{year}")]
        public async Task<GetUserBudgetReportQueryResponse> AddAuthor([FromQuery] int year)
        {
            return await _mediator.Send(new GetUserBudgetReportQuery()
            {
                Year = year,
            });
        }
    }
}
