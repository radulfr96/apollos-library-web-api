using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.IntegrationTests.Generators;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Business.Commands.DeleteBusinessCommand;
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
    public class DeleteBusinessCommandTest : TestBase
    {
        private readonly IDateTimeService _dateTime;
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;

        public DeleteBusinessCommandTest(TestFixture fixture) : base(fixture)
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
        public async Task DeleteBusiness()
        {
            Thread.CurrentPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Sid, "1"),
            });

            var BusinessGenerated = BusinessGenerator.GetGenericBusiness("AU", new Guid());

            _context.Business.Add(BusinessGenerated);
            _context.SaveChanges();

            var command = new DeleteBusinessCommand()
            {
                PubisherId = BusinessGenerated.BusinessId,
            };

            await _mediatr.Send(command);

            var Business = _context.Business.FirstOrDefault(p => p.BusinessId == command.PubisherId);

            Business.IsDeleted.Should().BeTrue();
        }
    }
}
