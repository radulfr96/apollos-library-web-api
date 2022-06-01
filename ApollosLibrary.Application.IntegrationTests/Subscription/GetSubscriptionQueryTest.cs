using ApollosLibrary.Application.Common.Enums;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Subscriptions.Queries.GetSubscriptionQuery;
using ApollosLibrary.Domain;
using Bogus;
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
using System.Threading.Tasks;
using Xunit;

namespace ApollosLibrary.Application.IntegrationTests.Subscription
{
    [Collection("IntegrationTestCollection")]
    public class GetSubscriptionQueryTest : TestBase
    {

        private readonly ApollosLibraryContext _context;
        private readonly IDateTimeService _dateTimeService;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;

        public GetSubscriptionQueryTest(TestFixture fixture) : base(fixture)
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
        public async Task GetSubscription_NewSubscription()
        {
            var userId = Guid.NewGuid();

            var httpContext = new TestHttpContext
            {
                User = new TestPrincipal(new Claim[]
                {
                    new Claim("userid", userId.ToString()),
                    new Claim("username", new Faker().Internet.Email()),
                }),
            };

            _contextAccessor.HttpContext = httpContext;

            var response = await _mediatr.Send(new GetSubscriptionQuery());

            response.Should().BeEquivalentTo(new GetSubscriptionQueryDTO()
            {
                JoinDate = _dateTimeService.Now,
                SubscriptionAdmin = true,
                SubscriptionName = "Signed Up",
                SubscriptionType = SubscriptionTypeEnum.SignedUp,
            }, opt => opt.Excluding(f => f.SubscriptionUsers));
        }

        [Fact]
        public async Task GetSubscription_ExistingSubscription()
        {
            var userId = Guid.NewGuid();
            var email = new Faker().Internet.Email();

            var httpContext = new TestHttpContext
            {
                User = new TestPrincipal(new Claim[]
                {
                    new Claim("userid", userId.ToString()),
                    new Claim("username", email),
                }),
            };

            _contextAccessor.HttpContext = httpContext;

            UserSubscription userSubscription = new UserSubscription()
            {
                Email = email,
                UserId = userId,
            };

            UserSubscription userSubscriptionOtherUser = new UserSubscription()
            {
                Email = new Faker().Internet.Email(),
                UserId = Guid.NewGuid(),
            };

            Domain.Subscription subscription = new Domain.Subscription()
            {
                ExpiryDate = _dateTimeService.Now.AddMonths(1),
                SubscriptionAdmin = userSubscription.UserId,
                StripeSubscriptionId = new Faker().Random.AlphaNumeric(6),
                SubscriptionDate = _dateTimeService.Now,
                SubscriptionTypeId = (int)new Faker().Random.Enum<SubscriptionTypeEnum>(),
                SubscriptionUsers = new List<UserSubscription>()
                {
                    userSubscription,
                    userSubscriptionOtherUser,
                },
            };

            _context.Subscriptions.Add(subscription);
            _context.SaveChanges();

            var result = await _mediatr.Send(new GetSubscriptionQuery());

            result.Should().BeEquivalentTo(new GetSubscriptionQueryDTO()
            {
                Expiry = subscription.ExpiryDate,
                JoinDate = subscription.SubscriptionDate,
                SubscriptionAdmin = true,
                SubscriptionType = (SubscriptionTypeEnum)subscription.SubscriptionTypeId,
                SubscriptionUsers = new List<SubscriptionUserDTO>()
                {
                    new SubscriptionUserDTO()
                    {
                        Email = userSubscriptionOtherUser.Email,
                        UserId = userSubscriptionOtherUser.UserId,
                    }
                }
            }, opt => opt.Excluding(f => f.SubscriptionName));
        }
    }
}
