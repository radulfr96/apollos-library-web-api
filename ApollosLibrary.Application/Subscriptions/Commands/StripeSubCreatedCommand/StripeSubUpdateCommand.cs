using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Domain;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Subscriptions.Commands.StripeSubCreatedCommand
{
    public class StripeSubUpdateCommand : IRequest<StripeSubUpdateCommandDto>
    {
        public Stripe.Subscription StripeSubscription { get; set; }
    }

    public class StripeSubCreatedCommandHandler : IRequestHandler<StripeSubUpdateCommand, StripeSubUpdateCommandDto>
    {
        private readonly ISubscriptionUnitOfWork _subscriptionUnitOfWork;
        private readonly ILibraryUnitOfWork _libraryUnitOfWork;

        public StripeSubCreatedCommandHandler(ISubscriptionUnitOfWork subscriptionUnitOfWork, ILibraryUnitOfWork libraryUnitOfWork)
        {
            _subscriptionUnitOfWork = subscriptionUnitOfWork;
            _libraryUnitOfWork = libraryUnitOfWork;
        }

        public async Task<StripeSubUpdateCommandDto> Handle(StripeSubUpdateCommand request, CancellationToken cancellationToken)
        {
            var userId = request.StripeSubscription.Metadata.FirstOrDefault(r => r.Key == "userId");

            if (userId.Value == null)
            {
                throw new StripeSubscriptionMissingUserIdException();
            }

            UserSubscription sub = await _subscriptionUnitOfWork.SubscriptionDataLayer.GetUserSubscription(Guid.Parse(userId.Value));

            if (sub == null)
            {
                throw new SubscriptionNotFoundException(Guid.Parse(userId.Value));
            }

            if (request.StripeSubscription.CancelAtPeriodEnd)
            {
                sub.Subscription.ExpiryDate = request.StripeSubscription.CurrentPeriodEnd;
            }

            var subTypes = await _subscriptionUnitOfWork.SubscriptionDataLayer.GetSubscriptionTypes(true);

            var type = subTypes.FirstOrDefault(s => s.StripeProductId == request.StripeSubscription.Items.First().Plan.ProductId);

            if (type == null)
            {
                throw new SubscriptionTypeNotFoundException();
            }

            sub.Subscription.SubscriptionTypeId = type.SubscriptionTypeId;
            sub.Subscription.StripeCustomerId = request.StripeSubscription.CustomerId;
            var library = new Domain.Library()
            {
                UserId = Guid.Parse(userId.Value),
            };

            await _libraryUnitOfWork.LibraryDataLayer.AddLibrary(library);

            await _libraryUnitOfWork.Save();
            await _subscriptionUnitOfWork.Save();

            return new StripeSubUpdateCommandDto();
        }
    }
}
