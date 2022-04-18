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

namespace ApollosLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class GetBusinessQueryTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;

        public GetBusinessQueryTest(TestFixture fixture) : base(fixture)
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

            var BusinessGenerated = BusinessGenerator.GetGenericBusiness("AU", new Guid());

            _context.Business.Add(BusinessGenerated);
            _context.SaveChanges();

            var query = new GetBusinessQuery()
            {
                BusinessId = BusinessGenerated.BusinessId,
            };

            var result = await _mediatr.Send(query);

            result.Should().BeEquivalentTo(new GetBusinessQueryDto()
            {
                City = BusinessGenerated.City,
                CountryID = BusinessGenerated.CountryId,
                Name = BusinessGenerated.Name,
                Postcode = BusinessGenerated.Postcode,
                BusinessId = BusinessGenerated.BusinessId,
                State = BusinessGenerated.State,
                StreetAddress = BusinessGenerated.StreetAddress,
                Website = BusinessGenerated.Website,
            });
        }
    }
}
