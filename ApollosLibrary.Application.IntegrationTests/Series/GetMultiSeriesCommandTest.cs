using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.IntegrationTests.Generators;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Business.Queries.GetBusinesssQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ApollosLibrary.Domain;
using ApollosLibrary.Application.Series.Queries.GetMultiSeriesQuery;

namespace ApollosLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class GetMultiSeriesQueryTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;

        public GetMultiSeriesQueryTest(TestFixture fixture) : base(fixture)
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
        public async Task GetMultiSeriesQuery()
        {
            Thread.CurrentPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Sid, "1"),
            });

            var series1 = SeriesGenerator.GetSeries(new Guid());
            _context.Series.Add(series1);

            var series2 = SeriesGenerator.GetSeries(new Guid());
            _context.Series.Add(series2);

            var series3 = SeriesGenerator.GetSeries(new Guid());
            _context.Series.Add(series3);

            _context.SaveChanges();

            var countries = _context.Countries.ToList();

            var query = new GetMultiSeriesQuery()
            {
            };

            var result = await _mediatr.Send(query);

            result.Series.Should().ContainEquivalentOf(new SeriesListItemDTO()
            {
                SeriesId = series1.SeriesId,
                Name = series1.Name,
            });

            result.Series.Should().ContainEquivalentOf(new SeriesListItemDTO()
            {
                SeriesId = series2.SeriesId,
                Name = series2.Name,
            });

            result.Series.Should().ContainEquivalentOf(new SeriesListItemDTO()
            {
                SeriesId = series3.SeriesId,
                Name = series3.Name,
            });
        }
    }
}
