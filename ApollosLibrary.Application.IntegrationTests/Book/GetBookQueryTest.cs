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

namespace ApollosLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class GetBookQueryTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;

        public GetBookQueryTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;
            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContext>();
        }

        [Fact]
        public async Task GetBookQuery()
        {
            Thread.CurrentPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Sid, "1"),
            });

            var Business = BusinessGenerator.GetGenericBusiness("AU", new Guid());
            _context.Business.Add(Business);
            _context.SaveChanges();

            var book = BookGenerator.GetGenericPhysicalBook(new Guid());
            book.BusinessId = Business.BusinessId;
            _context.Books.Add(book);

            _context.SaveChanges();

            var command = new GetBookQuery()
            {
                BookId = book.BookId,
            };

            var result = await _mediatr.Send(command);

            result.Should().BeEquivalentTo(new GetBookQueryDto()
            {
                BookId = book.BookId,
                Edition = book.Edition,
                FictionTypeId = book.FictionTypeId,
                FormTypeId = book.FormTypeId,
                ISBN = book.Isbn,
                PublicationFormatId = book.PublicationFormatId,
                Subtitle = book.Subtitle,
                Title = book.Title,
                BusinessId = Business.BusinessId,
            });
        }
    }
}
