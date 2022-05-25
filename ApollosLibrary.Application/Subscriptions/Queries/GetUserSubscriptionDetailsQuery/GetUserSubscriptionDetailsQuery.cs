using ApollosLibrary.Application.Common.Enums;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Subscriptions.Queries.GetUserSubscriptionDetailsQuery
{
    public class GetUserSubscriptionDetailsQuery : IRequest<GetUserSubscriptionDetailsQueryDto>
    {
    }

    public class GetUserSubscriptionDetailsQueryHandler : IRequestHandler<GetUserSubscriptionDetailsQuery, GetUserSubscriptionDetailsQueryDto>
    {
        private readonly IUserService _userService;
        private readonly ISubscriptionUnitOfWork _subscriptionUnitOfWork;

        public GetUserSubscriptionDetailsQueryHandler(IUserService userService, ISubscriptionUnitOfWork subscriptionUnitOfWork)
        {
            _userService = userService;
            _subscriptionUnitOfWork = subscriptionUnitOfWork;
        }

        public async Task<GetUserSubscriptionDetailsQueryDto> Handle(GetUserSubscriptionDetailsQuery request, CancellationToken cancellationToken)
        {
            var userSubscription = await _subscriptionUnitOfWork.SubscriptionDataLayer.GetUserSubscription(_userService.GetUserId());

            return new GetUserSubscriptionDetailsQueryDto()
            {
                Expiry = userSubscription.Subscription.ExpiryDate,
                SubscriptionType = (SubscriptionTypeEnum)userSubscription.Subscription.SubscriptionTypeId,
            };
        }
    }
}
