using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Moderation.Commands.AddEntryReportCommand;
using ApollosLibrary.Domain;
using ApollosLibrary.Domain.Enums;
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

namespace ApollosLibrary.Application.IntegrationTests.Moderation
{
    [Collection("IntegrationTestCollection")]
    public class AddEntryReportCommandTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IDateTimeService _dateTimeService;

        public AddEntryReportCommandTest(TestFixture fixture) : base(fixture)
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
        public async Task AddReportEntryCommand()
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

            var command = new AddEntryReportCommand()
            {
                CreatedBy = entryCreatedUserId,
                EntryRecordId = 1,
                EntryType = EntryTypeEnum.Book,
            };

            var result = await _mediatr.Send(command);

            var entry = _context.EntryReports.FirstOrDefault(e => e.EntryRecordId == result.ReportEntryId);

            entry.Should().BeEquivalentTo(new EntryReport()
            {
                CreatedBy = entryCreatedUserId,
                EntryRecordId = command.EntryRecordId,
                EntryTypeId = (int)command.EntryType,
                EntryReportStatusId = (int)EntryReportStatusEnum.Open,
                ReportedBy = userID,
                ReportedDate = _dateTimeService.Now,
                EntryReportId = result.ReportEntryId,
            }, opt => opt
            .Excluding(f => f.EntryReportStatus)
            .Excluding(f => f.EntryType)
            .Excluding(f => f.EntryRecordId));
        }
    }
}
