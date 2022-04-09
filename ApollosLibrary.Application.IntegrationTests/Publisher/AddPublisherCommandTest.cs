using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.IntegrationTests.Generators;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Publisher.Commands.AddPublisherCommand;
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
    public class AddPublisherCommandTest : TestBase
    {
        private readonly IDateTimeService _dateTime;
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;

        public AddPublisherCommandTest(TestFixture fixture) : base(fixture)
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
        public async Task AddPublisherCommand()
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

            var publisherGenerated = PublisherGenerator.GetGenericPublisher("AU", userID);

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

            publisher.Should().BeEquivalentTo(new Domain.Publisher()
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
