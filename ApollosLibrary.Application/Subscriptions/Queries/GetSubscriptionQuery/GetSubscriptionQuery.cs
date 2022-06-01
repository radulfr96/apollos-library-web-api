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

namespace ApollosLibrary.Application.Subscriptions.Queries.GetSubscriptionQuery
{
    public class GetSubscriptionQuery : IRequest<GetSubscriptionQueryDTO>
    {
    }

    public class GetSubscriptionQueryHandler : IRequestHandler<GetSubscriptionQuery, GetSubscriptionQueryDTO>
    {
        private readonly IUserService _userService;
        private readonly ISubscriptionUnitOfWork _subscriptionUnitOfWork;
        private readonly IDateTimeService _dateTimeService;

        public GetSubscriptionQueryHandler(
            IUserService userService
            , ISubscriptionUnitOfWork subscriptionUnitOfWork
            , IDateTimeService dateTimeService)
        {
            _userService = userService;
            _subscriptionUnitOfWork = subscriptionUnitOfWork;
            _dateTimeService = dateTimeService;
        }

        public async Task<GetSubscriptionQueryDTO> Handle(GetSubscriptionQuery request, CancellationToken cancellationToken)
        {
            var response = new GetSubscriptionQueryDTO();

            var userId = _userService.GetUserId();

            var subscription = await _subscriptionUnitOfWork.SubscriptionDataLayer.GetUserSubscription(userId);

            if (subscription == null)
            {
                var subType = await _subscriptionUnitOfWork.SubscriptionDataLayer.GetSubscriptionType((int)SubscriptionTypeEnum.SignedUp);

                subscription = new Domain.UserSubscription()
                {
                    Email = _userService.GetUserEmail(),
                    UserId = userId,
                    Subscription = new Domain.Subscription()
                    {
                        SubscriptionAdmin = userId,
                        SubscriptionDate = _dateTimeService.Now,
                        SubscriptionTypeId = (int)SubscriptionTypeEnum.SignedUp,
                        SubscriptionType = subType,
                        SubscriptionId = Guid.NewGuid(),
                        SubscriptionUsers = new List<Domain.UserSubscription>(),
                    }
                };

                await _subscriptionUnitOfWork.SubscriptionDataLayer.AddUserSubscription(subscription);
                await _subscriptionUnitOfWork.Save();
            }

            response.JoinDate = subscription.Subscription.SubscriptionDate;
            response.SubscriptionName = subscription.Subscription.SubscriptionType.SubscriptionName;
            response.Expiry = subscription.Subscription.ExpiryDate;
            response.SubscriptionType = (SubscriptionTypeEnum)subscription.Subscription.SubscriptionTypeId;
            response.SubscriptionAdmin = subscription.Subscription.SubscriptionAdmin == userId;
            response.SubscriptionUsers = subscription.Subscription.SubscriptionUsers
                                                    .Where(su => su.UserId != userId)
                                                    .Select(su => new SubscriptionUserDTO()
                                                    {
                                                        Email = su.Email,
                                                        UserId = su.UserId,
                                                    })
                                                    .ToList();

            return response;
        }
    }
}
