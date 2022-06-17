using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.IntegrationTests.Generators;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Business.Queries.GetBusinessQuery;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ApollosLibrary.Domain;
using ApollosLibrary.Application.Business.Queries.GetBusinessRecordQuery;
using ApollosLibrary.Application.Series.Queries.GetSeriesRecordQuery;
using System.Collections.Generic;

namespace ApollosLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class GetSeriesRecordQueryTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;

        public GetSeriesRecordQueryTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(new DateTime(2021, 02, 07));
            services.AddSingleton(mockDateTimeService.Object);

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContext>();
        }

        [Fact]
        public async Task GetSeriesRecordQuery()
        {
            Thread.CurrentPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Sid, "1"),
            });

            var seriesGenerated = SeriesGenerator.GetSeries(Guid.NewGuid());
            var seriesRecord = new SeriesRecord()
            {
                CreatedBy = seriesGenerated.CreatedBy,
                CreatedDate = seriesGenerated.CreatedDate,
                IsDeleted = seriesGenerated.IsDeleted,
                Name = seriesGenerated.Name,
                ReportedVersion = true,
                SeriesId = seriesGenerated.SeriesId,
            };

            seriesGenerated.SeriesRecords = new List<SeriesRecord>()
            {
                seriesRecord
            };

            _context.Series.Add(seriesGenerated);
            _context.SaveChanges();

            var query = new GetSeriesRecordQuery()
            {
                SeriesRecordId = seriesRecord.SeriesRecordId,
            };

            var result = await _mediatr.Send(query);

            result.Should().BeEquivalentTo(new GetSeriesRecordQueryDto()
            {
                Name = seriesGenerated.Name,
                SeriesId = seriesGenerated.SeriesId,
            });
        }
    }
}
