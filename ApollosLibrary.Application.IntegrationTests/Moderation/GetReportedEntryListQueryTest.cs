﻿using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Moderation.Queries;
using ApollosLibrary.Application.Moderation.Queries.GetReportedEntryListQuery;
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
    public class GetReportedEntryListQueryTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IDateTimeService _dateTimeService;

        public GetReportedEntryListQueryTest(TestFixture fixture) : base(fixture)
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
            var entryReportUser2 = Guid.NewGuid();
            var entryUser2 = Guid.NewGuid();
            var entryUser3 = Guid.NewGuid();

            var report1 = new EntryReport()
            {
                CreatedBy = entryUser1,
                CreatedDate = _dateTimeService.Now,
                EntryId = new Faker().Random.Int(1),
                EntryType = new Faker().Random.Enum<EntryTypeEnum>(),
                ReportedBy = entryReportUser1,
                ReportedDate = _dateTimeService.Now.AddDays(1),
            };
            _context.EntryReports.Add(report1);

            var report2 = new EntryReport()
            {
                CreatedBy = entryUser2,
                CreatedDate = _dateTimeService.Now,
                EntryId = new Faker().Random.Int(1),
                EntryType = new Faker().Random.Enum<EntryTypeEnum>(),
                ReportedBy = entryReportUser2,
                ReportedDate = _dateTimeService.Now.AddDays(1),
            };
            _context.EntryReports.Add(report2);

            var report3 = new EntryReport()
            {
                CreatedBy = entryUser3,
                CreatedDate = _dateTimeService.Now,
                EntryId = new Faker().Random.Int(1),
                EntryType = new Faker().Random.Enum<EntryTypeEnum>(),
                ReportedBy = entryReportUser1,
                ReportedDate = _dateTimeService.Now.AddDays(1),
            };
            _context.EntryReports.Add(report3);

            _context.SaveChanges();

            _contextAccessor.HttpContext = httpContext;

            var command = new GetReportedEntryListQuery()
            {
            };

            var result = await _mediatr.Send(command);

            result.EntryReports.Should().ContainEquivalentOf(new EntryReportListItem()
            {
                CreatedBy = report1.CreatedBy,
                CreatedDate = report1.CreatedDate,
                EntryId = report1.EntryId,
                EntryType = report1.EntryType,
                ReportedBy = report1.ReportedBy,
                ReportedDate = report1.ReportedDate,
                ReportId = report1.EntryReportId,
            });

            result.EntryReports.Should().ContainEquivalentOf(new EntryReportListItem()
            {
                CreatedBy = report2.CreatedBy,
                CreatedDate = report2.CreatedDate,
                EntryId = report2.EntryId,
                EntryType = report2.EntryType,
                ReportedBy = report2.ReportedBy,
                ReportedDate = report2.ReportedDate,
                ReportId = report2.EntryReportId,
            });
            result.EntryReports.Should().ContainEquivalentOf(new EntryReportListItem()
            {
                CreatedBy = report3.CreatedBy,
                CreatedDate = report3.CreatedDate,
                EntryId = report3.EntryId,
                EntryType = report3.EntryType,
                ReportedBy = report3.ReportedBy,
                ReportedDate = report3.ReportedDate,
                ReportId = report3.EntryReportId,
            });
        }
    }
}
