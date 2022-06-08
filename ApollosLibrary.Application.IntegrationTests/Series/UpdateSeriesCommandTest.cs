using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.IntegrationTests.Generators;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Business.Commands.UpdateBusinessCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ApollosLibrary.Domain;
using ApollosLibrary.Application.Series.Commands.AddSeriesCommand;
using Bogus;
using ApollosLibrary.Application.Series.Commands.UpdateSeriesCommand;
using Microsoft.EntityFrameworkCore;

namespace ApollosLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class UpdateSeriesCommandTest : TestBase
    {
        private readonly IDateTimeService _dateTime;
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;

        public UpdateSeriesCommandTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(new DateTime(2021, 02, 07));
            _dateTime = mockDateTimeService.Object;
            services.AddSingleton(mockDateTimeService.Object);

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContext>();
            _contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
        }

        [Fact]
        public async Task UpdateSeriesCommand()
        {
            var userID = Guid.NewGuid();

            var httpContext = new TestHttpContext
            {
                User = new TestPrincipal(new Claim[]
                {
                    new Claim("userid", userID.ToString()),
                })
            };

            _contextAccessor.HttpContext = httpContext;

            var seriesCommand = new AddSeriesCommand()
            {
                Name = new Faker().Random.AlphaNumeric(10),
            };

            var seriesResult = await _mediatr.Send(seriesCommand);

            var seriesGenerated = SeriesGenerator.GetSeries(userID);

            var command = new UpdateSeriesCommand()
            {
                SeriesId = seriesResult.SeriesId,
                Name = new Faker().Random.AlphaNumeric(10),
            };

            var result = await _mediatr.Send(command);

            var series = _context.Series.Include(s => s.SeriesRecords).FirstOrDefault(p => p.SeriesId == seriesResult.SeriesId);

            series.Should().BeEquivalentTo(new Domain.Series()
            {
                SeriesId = seriesResult.SeriesId,
                CreatedBy = series.CreatedBy,
                CreatedDate = series.CreatedDate,
                Name = command.Name,
            }, opt => opt.Excluding(f => f.Books).Excluding(f => f.SeriesRecords));

            series.SeriesRecords.Last(r => r.SeriesId == series.SeriesId).Should().BeEquivalentTo(new SeriesRecord()
            {
                CreatedBy = userID,
                CreatedDate = _dateTime.Now,
                IsDeleted = false,
                Name = command.Name,
                ReportedVersion = false,
                SeriesId = series.SeriesId,
                SeriesRecordId = series.SeriesRecords.Last(r => r.SeriesId == series.SeriesId).SeriesRecordId
            }, opt => opt.Excluding(f => f.Series));
        }
    }
}
