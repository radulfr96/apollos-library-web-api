using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.BillingPortal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Subscriptions.Queries.GetCustomerPortalQuery
{
    public class GetCustomerPortalQuery : IRequest<GetCustomerPortalQueryDto>
    {
    }

    public class GetCustomerPortalQueryHandler : IRequestHandler<GetCustomerPortalQuery, GetCustomerPortalQueryDto>
    {
        private readonly IConfiguration _configuration;
        private readonly ISubscriptionUnitOfWork _subscriptionUnitOfWork;
        private readonly IUserService _userService;

        public GetCustomerPortalQueryHandler(
            IConfiguration configuration
            , ISubscriptionUnitOfWork subscriptionUnitOfWork
            , IUserService userService)
        {
            _configuration = configuration;
            _subscriptionUnitOfWork = subscriptionUnitOfWork;
            _userService = userService;
        }

        public async Task<GetCustomerPortalQueryDto> Handle(GetCustomerPortalQuery request, CancellationToken cancellationToken)
        {
            string apiKey = _configuration.GetSection("Stripe_APIKey").Value;
            var client = new StripeClient(apiKey);
            string frontEndURL = _configuration.GetSection("FrontEndURL").Value;

            var subscription = await _subscriptionUnitOfWork.SubscriptionDataLayer.GetUserSubscription(_userService.GetUserId());

            var options = new SessionCreateOptions
            {
                Customer = subscription.Subscription.StripeCustomerId,
                ReturnUrl = frontEndURL,
            };
            var service = new SessionService(client);
            var session = await service.CreateAsync(options);

            return new GetCustomerPortalQueryDto()
            {
                SessionUrl = session.Url,
            };
        }
    }
}
