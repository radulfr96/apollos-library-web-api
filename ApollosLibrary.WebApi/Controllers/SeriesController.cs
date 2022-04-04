using ApollosLibrary.Application.Series.Queries.GetMultiSeriesQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ApollosLibrary.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class SeriesController : BaseApiController
    {
        private readonly IMediator _mediator;

        public SeriesController(IMediator mediator, IConfiguration configuration) : base(configuration)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Used to get series
        /// </summary>
        /// <returns>Response that indicates the result</returns>
        [HttpGet("")]
        public async Task<GetMultiSeriesQueryDto> GetSeries()
        {
            return await _mediator.Send(new GetMultiSeriesQuery());
        }
    }
}
