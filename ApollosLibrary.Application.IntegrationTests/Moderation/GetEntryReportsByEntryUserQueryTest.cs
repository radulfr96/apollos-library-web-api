using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Moderation.Queries;
using ApollosLibrary.Application.Moderation.Queries.GetEntryReportsByEntryUserQuery;
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
    public class GetEntryReportsByEntryUserQueryTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IDateTimeService _dateTimeService;

        public GetEntryReportsByEntryUserQueryTest(TestFixture fixture) : base(fixture)
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
        public async Task GetListOfReportsOfEntriesByUser()
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
            var entryReportUser2 = Guid.NewGuid();
            var entryUser2 = Guid.NewGuid();

            var report1 = new EntryReport()
            {
                CreatedBy = entryUser1,
                CreatedDate = _dateTimeService.Now,
                EntryId = new Faker().Random.Int(1),
                EntryTypeId = (int)new Faker().Random.Enum<EntryTypeEnum>(),
                EntryReportStatusId = (int)new Faker().Random.Enum<EntryReportStatusEnum>(),
                ReportedBy = entryReportUser1,
                ReportedDate = _dateTimeService.Now.AddDays(1),
            };
            _context.EntryReports.Add(report1);

            var report2 = new EntryReport()
            {
                CreatedBy = entryUser2,
                CreatedDate = _dateTimeService.Now,
                EntryId = new Faker().Random.Int(1),
                EntryTypeId = (int)new Faker().Random.Enum<EntryTypeEnum>(),
                EntryReportStatusId = (int)new Faker().Random.Enum<EntryReportStatusEnum>(),
                ReportedBy = entryReportUser2,
                ReportedDate = _dateTimeService.Now.AddDays(1),
            };
            _context.EntryReports.Add(report2);

            var report3 = new EntryReport()
            {
                CreatedBy = entryUser1,
                CreatedDate = _dateTimeService.Now,
                EntryId = new Faker().Random.Int(1),
                EntryTypeId = (int)new Faker().Random.Enum<EntryTypeEnum>(),
                EntryReportStatusId = (int)new Faker().Random.Enum<EntryReportStatusEnum>(),
                ReportedBy = entryReportUser1,
                ReportedDate = _dateTimeService.Now.AddDays(1),
            };
            _context.EntryReports.Add(report3);

            _context.SaveChanges();

            _contextAccessor.HttpContext = httpContext;

            var command = new GetEntryReportsByEntryUserQuery()
            {
                UserId = entryUser1
            };

            var result = await _mediatr.Send(command);

            result.EntryReports.Should().BeEquivalentTo(new List<EntryReportListItem>()
            {
                new EntryReportListItem()
                {
                    CreatedBy = report1.CreatedBy,
                    CreatedDate = report1.CreatedDate,
                    EntryRecordId = report1.EntryRecordId,
                    EntryTypeId = report1.EntryTypeId,
                    EntryStatusId = report1.EntryReportStatusId,
                    ReportedBy = report1.ReportedBy,
                    ReportedDate = report1.ReportedDate,
                    ReportId = report1.EntryReportId,
                },
                new EntryReportListItem()
                {
                    CreatedBy = report3.CreatedBy,
                    CreatedDate = report3.CreatedDate,
                    EntryRecordId = report3.EntryRecordId,
                    EntryTypeId = report3.EntryTypeId,
                    EntryStatusId = report3.EntryReportStatusId,
                    ReportedBy = report3.ReportedBy,
                    ReportedDate = report3.ReportedDate,
                    ReportId = report3.EntryReportId,
                },
            }, opt => opt.Excluding(f => f.EntryType).Excluding(f => f.EntryStatus));
        }
    }
}
