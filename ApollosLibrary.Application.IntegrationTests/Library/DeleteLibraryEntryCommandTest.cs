using ApollosLibrary.Application.IntegrationTests.Generators;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Library.Commands.DeleteLibraryEntryCommand;
using ApollosLibrary.Domain;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApollosLibrary.Application.IntegrationTests.Library
{
    [Collection("IntegrationTestCollection")]
    public class DeleteLibraryEntryCommandTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;

        public DeleteLibraryEntryCommandTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(LocalDateTime.FromDateTime(new DateTime(2021, 02, 07)));
            services.AddSingleton(mockDateTimeService.Object);

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContext>();
            _contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
        }

        [Fact]
        public async Task DeleteLibraryEntryCommand()
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

            var bookGenerated = BookGenerator.GetGenericPhysicalBook(userID);
            var entry = new LibraryEntry()
            {
                Book = bookGenerated,
                Quantity = 2,
            };

            var library = new Domain.Library()
            {
                LibraryEntries = new List<LibraryEntry>()
                {
                    entry,
                },
                UserId = userID,
            };

            _context.Libraries.Add(library);

            _context.SaveChanges();

            var command = new DeleteLibraryEntryCommand()
            {
                LibraryEntryId = entry.EntryId,
            };

            var result = await _mediatr.Send(command);

            result.Should().NotBeNull();

            var entryAfter = _context.LibraryEntries.FirstOrDefault(e => e.EntryId == command.LibraryEntryId);
            entryAfter.Should().BeNull();
        }
    }
}
