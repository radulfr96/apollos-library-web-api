using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.IntegrationTests.Generators;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Publisher.Queries.GetPublisherQuery;
using ApollosLibrary.Persistence.Model;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApollosLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class GetPublisherQueryTest : TestBase
    {
        private readonly IDateTimeService _dateTime;
        private readonly ApollosLibraryContextOld _context;
        private readonly IMediator _mediatr;

        public GetPublisherQueryTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(new DateTime(2021, 02, 07));
            _dateTime = mockDateTimeService.Object;
            services.AddSingleton(mockDateTimeService.Object);

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContextOld>();
        }

        [Fact]
        public async Task GetPublisherQuery()
        {
            Thread.CurrentPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Sid, "1"),
            });

            var publisherGenerated = PublisherGenerator.GetGenericPublisher("AU", new Guid());

            _context.Publishers.Add(publisherGenerated);
            _context.SaveChanges();

            var query = new GetPublisherQuery()
            {
                PublisherId = publisherGenerated.PublisherId,
            };

            var result = await _mediatr.Send(query);

            result.Should().BeEquivalentTo(new GetPublisherQueryDto()
            {
                City = publisherGenerated.City,
                CountryID = publisherGenerated.CountryId,
                Name = publisherGenerated.Name,
                Postcode = publisherGenerated.Postcode,
                PublisherId = publisherGenerated.PublisherId,
                State = publisherGenerated.State,
                StreetAddress = publisherGenerated.StreetAddress,
                Website = publisherGenerated.Website,
            });
        }
    }
}
