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
using ApollosLibrary.Application.Series.Queries.GetSeriesQuery;
using NodaTime;

namespace ApollosLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class GetSeriesQueryTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;

        public GetSeriesQueryTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(LocalDateTime.FromDateTime(new DateTime(2021, 02, 07)));
            services.AddSingleton(mockDateTimeService.Object);

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContext>();
        }

        [Fact]
        public async Task GetSeriesQuery()
        {
            Thread.CurrentPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Sid, "1"),
            });

            var userId = Guid.NewGuid();

            var seriesGenerated = SeriesGenerator.GetSeries(userId);

            _context.Series.Add(seriesGenerated);
            _context.SaveChanges();

            var query = new GetSeriesQuery()
            {
                SeriesId = seriesGenerated.SeriesId,
            };

            var result = await _mediatr.Send(query);

            result.Should().BeEquivalentTo(new GetSeriesQueryDto()
            {
                Name = seriesGenerated.Name,
                SeriesId = seriesGenerated.SeriesId,
                CreatedBy = userId,
            });
        }
    }
}
