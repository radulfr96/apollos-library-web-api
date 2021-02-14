using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MyLibrary.Application.Book.Queries.GetBooksQuery;
using MyLibrary.Application.Common.Enums;
using MyLibrary.Application.IntegrationTests.Generators;
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
    public class GetBooksQueryTest : TestBase
    {

        private readonly MyLibraryContext _context;
        private readonly IMediator _mediatr;

        public GetBooksQueryTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;
            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<MyLibraryContext>();
        }

        [Fact]
        public async Task GetBooksQuery()
        {
            Thread.CurrentPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Sid, "1"),
            });

            var publisher = PublisherGenerator.GetGenericPublisher("AU");
            _context.Publishers.Add(publisher);
            _context.SaveChanges();

            var book1 = BookGenerator.GetGenericPhysicalBook(1);
            book1.PublisherId = publisher.PublisherId;
            _context.Books.Add(book1);

            var book2 = BookGenerator.GetGenericPhysicalBook(1);
            book2.PublisherId = publisher.PublisherId;
            _context.Books.Add(book2);

            var book3 = BookGenerator.GetGenericPhysicalBook(1);
            book3.PublisherId = publisher.PublisherId;
            _context.Books.Add(book3);

            _context.SaveChanges();

            var fictionTypes = _context.FictionTypes.ToList();
            var formatTypes = _context.FormTypes.ToList();

            var command = new GetBooksQuery() { };

            var result = await _mediatr.Send(command);

            result.Should().BeEquivalentTo(new GetBooksQueryDto()
            {
                Books = new List<BookListItemDTO>()
                {
                    new BookListItemDTO()
                    {
                        BookID = book1.BookId,
                        eISBN = book1.EIsbn,
                        FictionType = fictionTypes.First(f => f.TypeId == book1.FictionTypeId).Name,
                        FormatType = formatTypes.First(f => f.TypeId == book1.FormTypeId).Name,
                        ISBN = book1.Isbn,
                        Title = book1.Title,
                    },
                    new BookListItemDTO()
                    {
                        BookID = book2.BookId,
                        eISBN = book2.EIsbn,
                        FictionType = fictionTypes.First(f => f.TypeId == book2.FictionTypeId).Name,
                        FormatType = formatTypes.First(f => f.TypeId == book2.FormTypeId).Name,
                        ISBN = book2.Isbn,
                        Title = book2.Title,
                    },
                    new BookListItemDTO()
                    {
                        BookID = book3.BookId,
                        eISBN = book3.EIsbn,
                        FictionType = fictionTypes.First(f => f.TypeId == book3.FictionTypeId).Name,
                        FormatType = formatTypes.First(f => f.TypeId == book3.FormTypeId).Name,
                        ISBN = book3.Isbn,
                        Title = book3.Title,
                    },
                }
            });
        }
    }
}
