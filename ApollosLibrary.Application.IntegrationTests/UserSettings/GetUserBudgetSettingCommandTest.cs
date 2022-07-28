using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.UserSettings.Commands.GetUserBudgetSettingsCommand;
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
    public class GetUserBudgetSettingCommandTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IDateTimeService _dateTimeService;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;

        public GetUserBudgetSettingCommandTest(TestFixture fixture) : base(fixture)
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
        public async Task GetBudgetSetting_NewSetting()
        {
            var userId = Guid.NewGuid();

            var httpContext = new TestHttpContext
            {
                User = new TestPrincipal(new Claim[]
                {
                    new Claim("userid", userId.ToString()),
                    new Claim("emailaddress", new Faker().Internet.Email()),
                }),
            };

            _contextAccessor.HttpContext = httpContext;

            var response = await _mediatr.Send(new GetUserBudgetSettingCommand());

            response.Should().BeEquivalentTo(new GetUserBudgetSettingCommandDto()
            {
                UserBudgetSettingId = response.UserBudgetSettingId,
                Year = _dateTimeService.Now.Year,
                YearlyBudget = 0.00m,
            });
        }

        [Fact]
        public async Task GetBudgetSetting_ExistingSetting()
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
                Year = _dateTimeService.Now.Year,
                YearlyBudget = 100.00m,
            };

            _context.UserBudgetSettings.Add(budgetSetting);
            _context.SaveChanges();

            var result = await _mediatr.Send(new GetUserBudgetSettingCommand());

            result.Should().BeEquivalentTo(new GetUserBudgetSettingCommandDto()
            {
                UserBudgetSettingId = budgetSetting.UserBudgetSettingId,
                Year = budgetSetting.Year,
                YearlyBudget = budgetSetting.YearlyBudget,
            });
        }

        [Fact]
        public async Task GetBudgetSetting_ExistingSettings_CreateNewSetting()
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

            _context.UserBudgetSettings.Add(budgetSetting);
            _context.SaveChanges();

            var result = await _mediatr.Send(new GetUserBudgetSettingCommand());

            result.Should().BeEquivalentTo(new GetUserBudgetSettingCommandDto()
            {
                UserBudgetSettingId = result.UserBudgetSettingId,
                Year = _dateTimeService.Now.Year,
                YearlyBudget = 0.00m,
            });
        }
    }
}
