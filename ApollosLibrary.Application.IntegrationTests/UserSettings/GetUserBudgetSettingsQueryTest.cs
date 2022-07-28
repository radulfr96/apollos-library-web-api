using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.UserSettings.Queries.GetUserBudgetSettingsQuery;
using ApollosLibrary.Domain;
using Bogus;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApollosLibrary.Application.IntegrationTests.UserSettings
{
    [Collection("IntegrationTestCollection")]
    public class GetUserBudgetSettingsQueryTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IDateTimeService _dateTimeService;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;

        public GetUserBudgetSettingsQueryTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(LocalDateTime.FromDateTime(new DateTime(2021, 02, 07)));
            _dateTimeService = mockDateTimeService.Object;
            services.AddSingleton(_dateTimeService);

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContext>();
            _contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
        }

        [Fact]
        public async Task GetBudgetSettings()
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

            Domain.UserBudgetSetting budgetSetting = new Domain.UserBudgetSetting()
            {
                UserId = userId,
                Year = _dateTimeService.Now.PlusYears(-1).Year,
                YearlyBudget = 100.00m,
            };

            Domain.UserBudgetSetting budgetSetting1 = new Domain.UserBudgetSetting()
            {
                UserId = userId,
                Year = _dateTimeService.Now.Year,
                YearlyBudget = 200.00m,
            };

            _context.UserBudgetSettings.Add(budgetSetting);
            _context.UserBudgetSettings.Add(budgetSetting1);
            _context.SaveChanges();

            var result = await _mediatr.Send(new GetUserBudgetSettingsQuery());

            result.Should().BeEquivalentTo(new GetUserBudgetSettingsQueryDto()
            {
                UserBudgetSettings = new List<Application.UserSettings.Queries.GetUserBudgetSettingsQuery.UserBudgetSetting>()
                {
                    new Application.UserSettings.Queries.GetUserBudgetSettingsQuery.UserBudgetSetting()
                    {
                        UserBudgetSettingId = budgetSetting.UserBudgetSettingId,
                        Year = budgetSetting.Year,
                        YearlyBudget = budgetSetting.YearlyBudget,
                    },
                    new Application.UserSettings.Queries.GetUserBudgetSettingsQuery.UserBudgetSetting()
                    {
                        UserBudgetSettingId = budgetSetting1.UserBudgetSettingId,
                        Year = budgetSetting1.Year,
                        YearlyBudget = budgetSetting1.YearlyBudget,
                    }
                }
            });
        }
    }
}
