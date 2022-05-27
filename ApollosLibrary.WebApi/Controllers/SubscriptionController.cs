using ApollosLibrary.Application.Subscriptions.Queries.GetSubscriptionTypesQuery;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ApollosLibrary.WebApi.Controllers
{
    /// <summary>
    /// Used to manage a users subscription
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : BaseApiController
    {
        private readonly IMediator _mediatr;

        public SubscriptionController(IConfiguration configuration, IMediator mediatr) : base (configuration)
        {
            _mediatr = mediatr;
        }

        /// <summary>
        /// Used to get subscriptions that are purchasable by users
        /// </summary>
        /// <returns>The subscriptions</returns>
        [AllowAnonymous]
        [HttpGet("")]
        public async Task<GetSubsriptionTypesQueryDto> GetPaidSubscriptions()
        {
            return await _mediatr.Send(new GetSubscriptionTypesQuery()
            {
                PurchasableOnly = true,
            });
        }
    }
}
