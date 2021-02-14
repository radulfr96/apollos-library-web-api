using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyLibrary.Application.IntegrationTests.Generators;
using MyLibrary.Application.Interfaces;
using MyLibrary.Application.Publisher.Commands.AddPublisherCommand;
using MyLibrary.Persistence.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MyLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class AddPublisherCommandTest : TestBase
    {
        private readonly IDateTimeService _dateTime;
        private readonly MyLibraryContext _context;
        private readonly IMediator _mediatr;

        public AddPublisherCommandTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(new DateTime(2021, 02, 07));
            _dateTime = mockDateTimeService.Object;
            services.AddSingleton(mockDateTimeService.Object);

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<MyLibraryContext>();
        }

        [Fact]
        public async Task AddPublisherCommand()
        {
            Thread.CurrentPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Sid, "1"),
            });

            var publisherGenerated = PublisherGenerator.GetGenericPublisher("AU", 1);

            var command = new AddPublisherCommand()
            {
                City = publisherGenerated.City,
                CountryID = publisherGenerated.CountryId,
                Name = publisherGenerated.Name,
                Postcode = publisherGenerated.Postcode,
                State = publisherGenerated.State,
                StreetAddress = publisherGenerated.StreetAddress,
                Website = publisherGenerated.Website,
            };

            var result = await _mediatr.Send(command);

            var publisher = _context.Publishers.FirstOrDefault(p => p.PublisherId == result.PublisherId);

            publisher.Should().BeEquivalentTo(new Persistence.Model.Publisher()
            {
                City = publisherGenerated.City,
                CountryId = publisherGenerated.CountryId,
                CreatedBy = publisherGenerated.CreatedBy,
                CreatedDate = _dateTime.Now,
                IsDeleted = false,
                Name = publisherGenerated.Name,
                Postcode = publisherGenerated.Postcode,
                PublisherId = result.PublisherId,
                State = publisherGenerated.State,
                StreetAddress = publisherGenerated.StreetAddress,
                Website = publisherGenerated.Website,
            }, opt => opt.Excluding(f => f.Country));
        }
    }
}
