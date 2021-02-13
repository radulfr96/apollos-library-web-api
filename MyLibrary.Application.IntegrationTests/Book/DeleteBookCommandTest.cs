using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyLibrary.Application.IntegrationTests.Generators;
using MyLibrary.Application.Interfaces;
using MyLibrary.Persistence.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MyLibrary.Application.IntegrationTests.Book
{
    [Collection("IntegrationTestCollection")]
    public class DeleteBookCommandTest : TestBase
    {
        private readonly IDateTimeService _dateTime;
        private readonly MyLibraryContext _context;
        private readonly IMediator _mediatr;
        public DeleteBookCommandTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(new DateTime(2021, 02, 07));
            services.AddSingleton(mockDateTimeService.Object);
            _dateTime = mockDateTimeService.Object;

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<MyLibraryContext>();
        }

        [Fact]
        public async Task DeleteBookCommand()
        {
            //Thread.CurrentPrincipal = new TestPrincipal(new Claim[]
            //{
            //    new Claim(ClaimTypes.Sid, "1"),
            //});

            //var publisher = PublisherGenerator.GetGenericPublisher("AU");
            //_context.Publishers.Add(publisher);

            //var command = new DeleteBookCommand

            //var result = await _mediatr.Send(command);

        }
    }
}
