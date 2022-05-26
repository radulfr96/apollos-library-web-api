using ApollosLibrary.Application.Common.Enums;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Subscriptions.Queries.GetUserSubscriptionDetailsQuery;
using ApollosLibrary.Domain;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
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
    public class GetUserSubscriptionDetailsQueryTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IDateTimeService _dateTimeService;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;

        public GetUserSubscriptionDetailsQueryTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(new DateTime(2021, 02, 07));
            _dateTimeService = mockDateTimeService.Object;
            services.AddSingleton(_dateTimeService);

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContext>();
            _contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
        }

        [Fact]
        public async Task GetBusinessQuery()
        {
            var userId = Guid.NewGuid();

            var httpContext = new TestHttpContext
            {
                User = new TestPrincipal(new Claim[]
                {
                    new Claim("userid", userId.ToString()),
                }),
            };

            _contextAccessor.HttpContext = httpContext;

            var userSubscription = new Domain.UserSubscription()
            {
                SubscriptionId = Guid.NewGuid(),
                UserId = userId,
                Subscription = new Domain.Subscription()
                {
                    SubscriptionTypeId = (int)SubscriptionTypeEnum.Individual,
                    ExpiryDate = _dateTimeService.Now.AddMonths(1),
                    JoinDate = _dateTimeService.Now,
                }
            };

            _context.UserSubscriptions.Add(userSubscription);
            _context.SaveChanges();

            var response = await _mediatr.Send(new GetUserSubscriptionDetailsQuery());

            response.Should().BeEquivalentTo(new GetUserSubscriptionDetailsQueryDto()
            {
                Expiry = userSubscription.Subscription.ExpiryDate,
                SubscriptionType = SubscriptionTypeEnum.Individual,
            });
        }
    }
}
