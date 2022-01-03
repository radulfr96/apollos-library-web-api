using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyLibrary.Application.IntegrationTests.Generators;
using MyLibrary.Application.Interfaces;
using MyLibrary.Application.Publisher.Commands.UpdatePublisherCommand;
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
    public class UpdatePublisherCommandTest : TestBase
    {
        private readonly IDateTimeService _dateTime;
        private readonly MyLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;

        public UpdatePublisherCommandTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(new DateTime(2021, 02, 07));
            _dateTime = mockDateTimeService.Object;
            services.AddSingleton(mockDateTimeService.Object);

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<MyLibraryContext>();
            _contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
        }

        [Fact]
        public async Task UpdatePublisherCommand()
        {
            var userID = Guid.NewGuid();

            var httpContext = new TestHttpContext();

            httpContext.User = new TestPrincipal(new Claim[]
            {
                new Claim("userid", userID.ToString()),
            });

            _contextAccessor.HttpContext = httpContext;

            var publisherGenerated = PublisherGenerator.GetGenericPublisher("AU", userID);

            _context.Publishers.Add(publisherGenerated);
            _context.SaveChanges();

            var newPublisherDetails = PublisherGenerator.GetGenericPublisher("US", userID);

            var command = new UpdatePublisherCommand()
            {
                City = newPublisherDetails.City,
                CountryID = newPublisherDetails.CountryId,
                Name = newPublisherDetails.Name,
                Postcode = newPublisherDetails.Postcode,
                PublisherId = publisherGenerated.PublisherId,
                State = newPublisherDetails.State,
                StreetAddress = newPublisherDetails.StreetAddress,
                Website = newPublisherDetails.Website,
            };

            await _mediatr.Send(command);

            var publisher = _context.Publishers.FirstOrDefault(p => p.PublisherId == publisherGenerated.PublisherId);

            publisher.Should().BeEquivalentTo(new Persistence.Model.Publisher()
            {
                City = newPublisherDetails.City,
                CountryId = newPublisherDetails.CountryId,
                CreatedBy = userID,
                CreatedDate = publisherGenerated.CreatedDate,
                IsDeleted = false,
                ModifiedBy = userID,
                ModifiedDate = _dateTime.Now,
                Name = newPublisherDetails.Name,
                Postcode = newPublisherDetails.Postcode,
                PublisherId = publisherGenerated.PublisherId,
                State = newPublisherDetails.State,
                StreetAddress = newPublisherDetails.StreetAddress,
                Website = newPublisherDetails.Website,
            }, opt => opt.Excluding(f => f.Country));
        }
    }
}
