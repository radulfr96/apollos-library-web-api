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
        public async Task GetBusinessQuery()
        {
            Thread.CurrentPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Sid, "1"),
            });

            var Business1 = BusinessGenerator.GetGenericBusiness("AU", new Guid());
            _context.Business.Add(Business1);

            var Business2 = BusinessGenerator.GetGenericBusiness("AU", new Guid());
            _context.Business.Add(Business2);

            var Business3 = BusinessGenerator.GetGenericBusiness("AU", new Guid());
            _context.Business.Add(Business3);

            _context.SaveChanges();

            var countries = _context.Countries.ToList();

            var query = new GetBusinesssQuery()
            {
            };

            var result = await _mediatr.Send(query);

            result.Businesss.Should().ContainEquivalentOf(new BusinessListItemDTO()
            {
                Country = countries.First(c => c.CountryId == Business1.CountryId).Name,
                Name = Business1.Name,
                BusinessId = Business1.BusinessId,
            });

            result.Businesss.Should().ContainEquivalentOf(new BusinessListItemDTO()
            {
                Country = countries.First(c => c.CountryId == Business2.CountryId).Name,
                Name = Business2.Name,
                BusinessId = Business2.BusinessId,
            });

            result.Businesss.Should().ContainEquivalentOf(new BusinessListItemDTO()
            {
                Country = countries.First(c => c.CountryId == Business3.CountryId).Name,
                Name = Business3.Name,
                BusinessId = Business3.BusinessId,
            });
        }
    }
}
