using ApollosLibrary.Application.Common.Enums;
using ApollosLibrary.Application.Subscriptions.Queries.GetSubscriptionTypesQuery;
using ApollosLibrary.Domain;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApollosLibrary.Application.IntegrationTests.Subscription
{
    [Collection("IntegrationTestCollection")]
    public class GetSubscriptionTypesQueryTest : TestBase
    {
        private readonly IMediator _mediatr;

        public GetSubscriptionTypesQueryTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
        }

        [Fact]
        public async Task GetAllSubscriptionTypes()
        {
            var query = new GetSubscriptionTypesQuery()
            {
                PurchasableOnly = false,
            };

            var response = await _mediatr.Send(query);

            response.SubscriptionTypes.Should().BeEquivalentTo(new List<SubscriptionTypeDTO>
            {
                new SubscriptionTypeDTO()
                {
                    Cost = 0.00m,
                    SubscriptionName = "Signed Up",
                    SubscriptionType = SubscriptionTypeEnum.SignedUp,
                    MaxUsers = 1,
                },
                new SubscriptionTypeDTO()
                {
                    Cost = 0.00m,
                    SubscriptionName = "Staff Member",
                    SubscriptionType = SubscriptionTypeEnum.Staff,
                    MaxUsers = 1,
                },
                new SubscriptionTypeDTO()
                {
                    Cost = 10.00m,
                    SubscriptionName = "Individual Subscription",
                    SubscriptionType = SubscriptionTypeEnum.Individual,
                    MaxUsers = 1,
                },
                new SubscriptionTypeDTO()
                {
                    Cost = 30.00m,
                    SubscriptionName = "Family Subscription",
                    SubscriptionType = SubscriptionTypeEnum.Family,
                    MaxUsers = 5,
                },
            });
        }

        [Fact]
        public async Task GetPurchasableSubscriptionTypes()
        {
            var query = new GetSubscriptionTypesQuery()
            {
                PurchasableOnly = true,
            };

            var response = await _mediatr.Send(query);

            response.SubscriptionTypes.Should().BeEquivalentTo(new List<SubscriptionTypeDTO>
            {
                new SubscriptionTypeDTO()
                {
                    Cost = 10.00m,
                    SubscriptionName = "Individual Subscription",
                    SubscriptionType = SubscriptionTypeEnum.Individual,
                    MaxUsers = 1,
                },
                new SubscriptionTypeDTO()
                {
                    Cost = 30.00m,
                    SubscriptionName = "Family Subscription",
                    SubscriptionType = SubscriptionTypeEnum.Family,
                    MaxUsers = 5,
                },
            });
        }
    }
}
