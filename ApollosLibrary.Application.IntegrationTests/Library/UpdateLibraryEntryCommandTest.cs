using ApollosLibrary.Application.Book.Commands.AddBookCommand;
using ApollosLibrary.Application.IntegrationTests.Generators;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Library.Commands.AddLibraryEntryCommand;
using ApollosLibrary.Application.Library.Commands.CreateLibraryCommand;
using ApollosLibrary.Application.Library.Commands.UpdateLibraryEntryCommand;
using ApollosLibrary.Domain;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
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
    public class UpdateLibraryEntryCommandTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;

        public UpdateLibraryEntryCommandTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(new DateTime(2021, 02, 07));
            services.AddSingleton(mockDateTimeService.Object);

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContext>();
            _contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
        }

        [Fact]
        public async Task UpdateLibraryEntryCommand()
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

            var createLibraryCommand = new CreateLibraryCommand() { };

            var createResult = await _mediatr.Send(createLibraryCommand);

            var bookGenerated = BookGenerator.GetGenericPhysicalBook(userID);

            var addBookCommand = new AddBookCommand()
            {
                Edition = bookGenerated.Edition,
                FictionTypeId = bookGenerated.FictionTypeId,
                FormTypeId = bookGenerated.FormTypeId,
                ISBN = bookGenerated.Isbn,
                PublicationFormatId = bookGenerated.PublicationFormatId,
                Subtitle = bookGenerated.Subtitle,
                Title = bookGenerated.Title,
            };

            var createBookResult = await _mediatr.Send(addBookCommand);

            var addLibraryEntryCommand = new AddLibraryEntryCommand()
            {
                Quantity = 1,
                BookId = createBookResult.BookId,
            };

            var addEntryResult = await _mediatr.Send(addLibraryEntryCommand);

            var entry = _context.LibraryEntries
                                  .FirstOrDefault(l => l.EntryId == addEntryResult.LibraryEntryId
                                  && l.BookId == createBookResult.BookId);

            entry.Should().NotBeNull();
            entry.Should().BeEquivalentTo(new Domain.LibraryEntry()
            {
                LibraryId = createResult.LibraryId,
                BookId = createBookResult.BookId,
                Quantity = addLibraryEntryCommand.Quantity,
            },
            opt => opt.Excluding(f => f.EntryId)
                      .Excluding(f => f.Book)
                      .Excluding(f => f.Library));

            var command = new UpdateLibraryEntryCommand()
            {
                LibraryEntryId = addEntryResult.LibraryEntryId,
                Quantity = 10,
            };

            await _mediatr.Send(command);

            entry = _context.LibraryEntries
                                  .FirstOrDefault(l => l.EntryId == addEntryResult.LibraryEntryId
                                  && l.BookId == createBookResult.BookId);

            entry.Should().BeEquivalentTo(new Domain.LibraryEntry()
            {
                BookId = createBookResult.BookId,
                EntryId = entry.EntryId,
                LibraryId = createResult.LibraryId,
                Quantity = command.Quantity,
            },
            opt => opt.Excluding(f => f.EntryId)
                      .Excluding(f => f.Book)
                      .Excluding(f => f.Library));
        }
    }
}
