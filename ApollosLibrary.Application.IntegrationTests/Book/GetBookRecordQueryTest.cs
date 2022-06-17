using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ApollosLibrary.Application.Book.Queries.GetBookQuery;
using ApollosLibrary.Application.IntegrationTests.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ApollosLibrary.Domain;
using ApollosLibrary.Application.Book.Queries.GetBookRecordQuery;

namespace ApollosLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class GetBookRecordQueryTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;

        public GetBookRecordQueryTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;
            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContext>();
        }

        [Fact]
        public async Task GetBookRecordQuery()
        {
            Thread.CurrentPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Sid, "1"),
            });

            var business = BusinessGenerator.GetGenericBusiness("AU", new Guid());
            _context.Business.Add(business);
            _context.SaveChanges();

            var book = BookGenerator.GetGenericPhysicalBook(new Guid());
            var bookRecord = new BookRecord()
            {
                BookId = book.BookId,
                BusinessId = book.BusinessId,
                CoverImage = book.CoverImage,
                CreatedBy = book.CreatedBy,
                CreatedDate = book.CreatedDate,
                Edition = book.Edition,
                EIsbn = book.EIsbn,
                FictionTypeId = book.FictionTypeId,
                FormTypeId = book.FormTypeId,
                Isbn = book.Isbn,
                IsDeleted = false,
                PublicationFormatId = book.PublicationFormatId,
                ReportedVersion = true,
                Subtitle = book.Subtitle,
                Title = book.Title,
            };

            book.BusinessId = business.BusinessId;
            book.BookRecords.Add(bookRecord);
            _context.Books.Add(book);

            _context.SaveChanges();

            var command = new GetBookRecordQuery()
            {
                BookRecordId = bookRecord.BookRecordId,
            };

            var result = await _mediatr.Send(command);

            result.Should().BeEquivalentTo(new GetBookRecordQueryDto()
            {
                BookId = book.BookId,
                ISBN = book.Isbn,
                Subtitle = book.Subtitle,
                Title = book.Title,
                EISBN = book.EIsbn,
                CoverImage = book.CoverImage,
            });
        }
    }
}
