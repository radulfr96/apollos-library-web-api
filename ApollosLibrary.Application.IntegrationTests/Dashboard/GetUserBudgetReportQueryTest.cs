using ApollosLibrary.Application.Dashboard.Queries.GetUserBudgetProgressQuery;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Domain;
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

namespace ApollosLibrary.Application.IntegrationTests.Dashboard
{
    [Collection("IntegrationTestCollection")]
    public class GetUserBudgetReportQueryTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IDateTimeService _dateTimeService;

        public GetUserBudgetReportQueryTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(LocalDateTime.FromDateTime(new DateTime(2021, 02, 07)));
            services.AddSingleton(mockDateTimeService.Object);
            _dateTimeService = mockDateTimeService.Object;

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContext>();
            _contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
        }

        [Fact]
        public async Task UserBudgetReportNoSettingOrRecordsInDB()
        {
            var userID = Guid.NewGuid();

            var httpContext = new TestHttpContext
            {
                User = new TestPrincipal(new Claim[]
                {
                    new Claim("userid", userID.ToString()),
                }),
            };

            _contextAccessor.HttpContext = httpContext;

            var result = await _mediatr.Send(new GetUserBudgetReportQuery()
            {
                Year = 2020,
            });

            result.Should().BeEquivalentTo(new GetUserBudgetReportQueryResponse()
            {
                MontlhyBudget = 0.00m,
                Amounts = new List<Amount>()
                {
                    new Amount()
                    {
                        Month = 1,
                        Spend = 0.00m,
                    },
                    new Amount()
                    {
                        Month = 2,
                        Spend = 0.00m,
                    },
                    new Amount()
                    {
                        Month = 3,
                        Spend = 0.00m,
                    },
                    new Amount()
                    {
                        Month = 4,
                        Spend = 0.00m,
                    },
                    new Amount()
                    {
                        Month = 5,
                        Spend = 0.00m,
                    },
                    new Amount()
                    {
                        Month = 6,
                        Spend = 0.00m,
                    },
                    new Amount()
                    {
                        Month = 7,
                        Spend = 0.00m,
                    },
                    new Amount()
                    {
                        Month = 8,
                        Spend = 0.00m,
                    },
                    new Amount()
                    {
                        Month = 9,
                        Spend = 0.00m,
                    },
                    new Amount()
                    {
                        Month = 10,
                        Spend = 0.00m,
                    },
                    new Amount()
                    {
                        Month = 11,
                        Spend = 0.00m,
                    },
                    new Amount()
                    {
                        Month = 12,
                        Spend = 0.00m,
                    },
                }
            });
        }

        [Fact]
        public async Task UserBudgetReportRecordsInDB()
        {
            var userID = Guid.NewGuid();

            var httpContext = new TestHttpContext
            {
                User = new TestPrincipal(new Claim[]
                {
                    new Claim("userid", userID.ToString()),
                }),
            };

            _contextAccessor.HttpContext = httpContext;
            _context.UserBudgetSettings.Add(new UserBudgetSetting()
            {
                UserId = userID,
                Year = _dateTimeService.Now.Year,
                YearlyBudget = 400.00m,
            });
            await _context.SaveChangesAsync();

            var result = await _mediatr.Send(new GetUserBudgetReportQuery()
            {
                Year = 2021,
            });

            result.Should().BeEquivalentTo(new GetUserBudgetReportQueryResponse()
            {
                MontlhyBudget = Math.Round(400.00m / 12, 2),
                Amounts = new List<Amount>()
                {
                    new Amount()
                    {
                        Month = 1,
                        Spend = 0.00m,
                    },
                    new Amount()
                    {
                        Month = 2,
                        Spend = 0.00m,
                    },
                    new Amount()
                    {
                        Month = 3,
                        Spend = 0.00m,
                    },
                    new Amount()
                    {
                        Month = 4,
                        Spend = 0.00m,
                    },
                    new Amount()
                    {
                        Month = 5,
                        Spend = 0.00m,
                    },
                    new Amount()
                    {
                        Month = 6,
                        Spend = 0.00m,
                    },
                    new Amount()
                    {
                        Month = 7,
                        Spend = 0.00m,
                    },
                    new Amount()
                    {
                        Month = 8,
                        Spend = 0.00m,
                    },
                    new Amount()
                    {
                        Month = 9,
                        Spend = 0.00m,
                    },
                    new Amount()
                    {
                        Month = 10,
                        Spend = 0.00m,
                    },
                    new Amount()
                    {
                        Month = 11,
                        Spend = 0.00m,
                    },
                    new Amount()
                    {
                        Month = 12,
                        Spend = 0.00m,
                    },
                }
            });
        }
    }
}
