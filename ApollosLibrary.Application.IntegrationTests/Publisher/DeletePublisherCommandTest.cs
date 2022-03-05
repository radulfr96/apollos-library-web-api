using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.IntegrationTests.Generators;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Publisher.Commands.DeletePublisherCommand;
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
    public class DeletePublisherCommandTest : TestBase
    {
        private readonly IDateTimeService _dateTime;
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;

        public DeletePublisherCommandTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(new DateTime(2021, 02, 07));
            _dateTime = mockDateTimeService.Object;
            services.AddSingleton(mockDateTimeService.Object);

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContext>();
        }

        [Fact]
        public async Task DeletePublisher()
        {
            Thread.CurrentPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Sid, "1"),
            });

            var publisherGenerated = PublisherGenerator.GetGenericPublisher("AU", new Guid());

            _context.Publishers.Add(publisherGenerated);
            _context.SaveChanges();

            var command = new DeletePublisherCommand()
            {
                PubisherId = publisherGenerated.PublisherId,
            };

            await _mediatr.Send(command);

            var publisher = _context.Publishers.FirstOrDefault(p => p.PublisherId == command.PubisherId);

            publisher.IsDeleted.Should().BeTrue();
        }
    }
}
