using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MyLibrary.Application.Book.Queries.GetBookQuery;
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

namespace MyLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class GetBookQueryTest : TestBase
    {
        private readonly MyLibraryContext _context;
        private readonly IMediator _mediatr;

        public GetBookQueryTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;
            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<MyLibraryContext>();
        }

        [Fact]
        public async Task GetBookQuery()
        {
            Thread.CurrentPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Sid, "1"),
            });

            var publisher = PublisherGenerator.GetGenericPublisher("AU", 1);
            _context.Publishers.Add(publisher);
            _context.SaveChanges();

            var book = BookGenerator.GetGenericPhysicalBook(1);
            book.PublisherId = publisher.PublisherId;
            _context.Books.Add(book);

            _context.SaveChanges();

            var command = new GetBookQuery()
            {
                BookId = book.BookId,
            };

            var result = await _mediatr.Send(command);

            result.Should().BeEquivalentTo(new GetBookQueryDto()
            {
                BookID = book.BookId,
                Edition = book.Edition,
                FictionType = book.FictionTypeId,
                FormType = book.FormTypeId,
                ISBN = book.Isbn,
                NumberInSeries = book.NumberInSeries,
                PublicationFormat = book.PublicationFormatId,
                SeriesID = book.SeriesId,
                Subtitle = book.Subtitle,
                Title = book.Title,
                Publisher = publisher.PublisherId,
            });
        }
    }
}
