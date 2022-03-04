using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.IntegrationTests.Generators;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Publisher.Queries.GetPublishersQuery;
using ApollosLibrary.Persistence.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApollosLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class GetPublishersCommandTest : TestBase
    {
        private readonly IDateTimeService _dateTime;
        private readonly ApollosLibraryContextOld _context;
        private readonly IMediator _mediatr;

        public GetPublishersCommandTest(TestFixture fixture) : base(fixture)
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

            var publisher1 = PublisherGenerator.GetGenericPublisher("AU", new Guid());
            _context.Publishers.Add(publisher1);

            var publisher2 = PublisherGenerator.GetGenericPublisher("AU", new Guid());
            _context.Publishers.Add(publisher2);

            var publisher3 = PublisherGenerator.GetGenericPublisher("AU", new Guid());
            _context.Publishers.Add(publisher3);

            _context.SaveChanges();

            var countries = _context.Countries.ToList();

            var query = new GetPublishersQuery()
            {
            };

            var result = await _mediatr.Send(query);

            result.Should().BeEquivalentTo(new GetPublishersQueryDto()
            {
                Publishers = new List<PublisherListItemDTO>()
                {
                    new PublisherListItemDTO()
                    {
                        Country = countries.First(c => c.CountryId == publisher1.CountryId).Name,
                        Name = publisher1.Name,
                        PublisherId = publisher1.PublisherId,
                    },
                    new PublisherListItemDTO()
                    {
                        Country = countries.First(c => c.CountryId == publisher2.CountryId).Name,
                        Name = publisher2.Name,
                        PublisherId = publisher2.PublisherId,
                    },
                    new PublisherListItemDTO()
                    {
                        Country = countries.First(c => c.CountryId == publisher3.CountryId).Name,
                        Name = publisher3.Name,
                        PublisherId = publisher3.PublisherId,
                    },
                },
            });
        }
    }
}
