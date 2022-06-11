using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Moderation.Queries;
using ApollosLibrary.Application.Moderation.Queries.GetEntryReportQuery;
using ApollosLibrary.Domain;
using ApollosLibrary.Domain.Enums;
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

namespace ApollosLibrary.Application.IntegrationTests.Moderation
{
    [Collection("IntegrationTestCollection")]
    public class GetEntryReportQueryTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IDateTimeService _dateTimeService;

        public GetEntryReportQueryTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(new DateTime(2021, 02, 07));
            services.AddSingleton(mockDateTimeService.Object);
            _dateTimeService = mockDateTimeService.Object;

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContext>();
            _contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
        }

        [Fact]
        public async Task GetReportListEntries()
        {
            var userID = Guid.NewGuid();

            var httpContext = new TestHttpContext
            {
                User = new TestPrincipal(new Claim[]
                {
                    new Claim("userid", userID.ToString()),
                }),
            };

            var entryReportUser1 = Guid.NewGuid();
            var entryUser1 = Guid.NewGuid();

            var report = new EntryReport()
            {
                CreatedBy = entryUser1,
                CreatedDate = _dateTimeService.Now,
                EntryId = new Faker().Random.Int(1),
                EntryTypeId = (int)new Faker().Random.Enum<EntryTypeEnum>(),
                EntryReportStatusId = (int)new Faker().Random.Enum<EntryReportStatusEnum>(),
                ReportedBy = entryReportUser1,
                ReportedDate = _dateTimeService.Now.AddDays(1),
            };
            _context.EntryReports.Add(report);

            _context.SaveChanges();

            _contextAccessor.HttpContext = httpContext;

            var command = new GetEntryReportQuery()
            {
                ReportEntryId = report.EntryReportId,
            };

            var result = await _mediatr.Send(command);

            result.Should().BeEquivalentTo(new GetEntryReportQueryDto()
            {
                CreatedBy = report.CreatedBy,
                CreatedDate = report.CreatedDate,
                EntryId = report.EntryId,
                EntryTypeId = report.EntryTypeId,
                EntryReportStatusId = report.EntryReportStatusId,
                ReportedBy = report.ReportedBy,
                ReportedDate = report.ReportedDate,
            }, opt => opt.Excluding(f => f.EntryType).Excluding(f => f.EntryStatus));
        }
    }
}
