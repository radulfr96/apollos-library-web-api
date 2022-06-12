using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Moderation.Commands.UpdateEntryReportCommand;
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
    public class UpdateEntryReportCommandTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly Faker _faker;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IDateTimeService _dateTimeService;

        public UpdateEntryReportCommandTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            _faker = new Faker();
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
        public async Task UpdateReportEntryCommand()
        {
            var userID = Guid.NewGuid();

            var httpContext = new TestHttpContext
            {
                User = new TestPrincipal(new Claim[]
                {
                    new Claim("userid", userID.ToString()),
                }),
            };

            var entryCreatedUserId = Guid.NewGuid();

            _contextAccessor.HttpContext = httpContext;

            var entryReport = new EntryReport()
            {
                CreatedBy = Guid.NewGuid(),
                CreatedDate = _dateTimeService.Now,
                ReportedBy = Guid.NewGuid(),
                ReportedDate = _dateTimeService.Now.AddDays(1),
                EntryId = _faker.Random.Int(1),
                EntryReportStatusId = (int)_faker.Random.Enum<EntryReportStatusEnum>(),
                EntryTypeId = (int)_faker.Random.Enum<EntryReportTypeEnum>(),
            };

            _context.EntryReports.Add(entryReport);
            _context.SaveChanges();

            var command = new UpdateEntryReportCommand()
            {
                EntryReportId = entryReport.EntryReportId,
                EntryReportStatus = EntryReportStatusEnum.Confirmed,
            };

            var result = await _mediatr.Send(command);

            var entry = _context.EntryReports.FirstOrDefault(e => e.EntryReportId == entryReport.EntryReportId);

            entry.Should().BeEquivalentTo(new EntryReport()
            {
                CreatedBy = entryReport.CreatedBy,
                CreatedDate = entryReport.CreatedDate,
                EntryId = entryReport.EntryId,
                EntryTypeId = entryReport.EntryTypeId,
                EntryReportStatusId = (int)EntryReportStatusEnum.Confirmed,
                ReportedBy = entryReport.ReportedBy,
                ReportedDate = entryReport.ReportedDate,
                EntryReportId = entryReport.EntryReportId,
            }, opt => opt.Excluding(f => f.EntryReportStatus).Excluding(f => f.EntryType));
        }
    }
}
