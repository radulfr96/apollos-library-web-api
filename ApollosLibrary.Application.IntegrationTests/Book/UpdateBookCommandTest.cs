using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.Book.Commands.UpdateBookCommand;
using ApollosLibrary.Application.IntegrationTests.Generators;
using ApollosLibrary.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ApollosLibrary.Domain;
using Microsoft.EntityFrameworkCore;

namespace ApollosLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class UpdateBookCommandTest : TestBase
    {
        private readonly IDateTimeService _dateTime;
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;

        public UpdateBookCommandTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(new DateTime(2021, 02, 07));
            _dateTime = mockDateTimeService.Object;
            services.AddSingleton(mockDateTimeService.Object);

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContext>();
            _contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
        }

        [Fact]
        public async Task UpdateBookCommand()
        {
            var userID = Guid.NewGuid();

            var httpContext = new TestHttpContext
            {
                User = new TestPrincipal(new Claim[]
                {
                    new Claim("userid", userID.ToString()),
                }),
            };

            _contextAccessor.HttpContext = httpContext;

            var publisher1 = PublisherGenerator.GetGenericPublisher("GB", userID);
            _context.Publishers.Add(publisher1);

            var publisher2 = PublisherGenerator.GetGenericPublisher("GB", userID);
            _context.Publishers.Add(publisher2);

            _context.SaveChanges();

            var author1 = AuthorGenerator.GetGenericAuthor(userID, "GB");
            _context.Authors.Add(author1);

            var author2 = AuthorGenerator.GetGenericAuthor(userID, "GB");
            _context.Authors.Add(author2);

            var author3 = AuthorGenerator.GetGenericAuthor(userID, "GB");
            _context.Authors.Add(author3);

            var genre1 = GenreGenerator.GetGenre(userID);
            _context.Genres.Add(genre1);

            var genre2 = GenreGenerator.GetGenre(userID);
            _context.Genres.Add(genre2);

            var genre3 = GenreGenerator.GetGenre(userID);
            _context.Genres.Add(genre3);

            _context.SaveChanges();

            var bookGenerated = BookGenerator.GetGenericPhysicalBook(userID);
            bookGenerated.PublisherId = publisher1.PublisherId;
            bookGenerated.Genres = new List<Domain.Genre>()
            {
                genre1,
                genre2,
            };

            bookGenerated.Authors = new List<Domain.Author>()
            {
                author1,
                author2
            };

            _context.Books.Add(bookGenerated);
            _context.SaveChanges();

            var newBookDetails = BookGenerator.GetGenericPhysicalBook(userID);

            var command = new UpdateBookCommand()
            {
                Authors = new List<int>()
                {
                    author2.AuthorId,
                    author3.AuthorId,
                },
                BookId = bookGenerated.BookId,
                Edition = newBookDetails.Edition,
                EISBN = newBookDetails.EIsbn,
                FictionTypeId = newBookDetails.FictionTypeId,
                FormTypeId = newBookDetails.FormTypeId,
                Genres = new List<int>()
                {
                    genre2.GenreId,
                    genre3.GenreId,
                },
                ISBN = newBookDetails.Isbn,
                NumberInSeries = newBookDetails.NumberInSeries,
                PublicationFormatId = newBookDetails.PublicationFormatId,
                PublisherId = publisher2.PublisherId,
                SeriesID = newBookDetails.SeriesId,
                Subtitle = newBookDetails.Subtitle,
                Title = newBookDetails.Title,
            };

            var result = await _mediatr.Send(command);

            var book = _context.Books.FirstOrDefault(b => b.BookId == bookGenerated.BookId);

            book.Should().BeEquivalentTo(new Domain.Book()
            {
                BookId = bookGenerated.BookId,
                CreatedBy = userID,
                CreatedDate = bookGenerated.CreatedDate,
                Edition = newBookDetails.Edition,
                EIsbn = newBookDetails.EIsbn,
                FictionTypeId = newBookDetails.FictionTypeId,
                FormTypeId = newBookDetails.FormTypeId,
                Isbn = newBookDetails.Isbn,
                ModifiedBy = userID,
                ModifiedDate = _dateTime.Now,
                NumberInSeries = newBookDetails.NumberInSeries,
                PublicationFormatId = newBookDetails.PublicationFormatId,
                PublisherId = publisher2.PublisherId,
                SeriesId = newBookDetails.SeriesId,
                Subtitle = newBookDetails.Subtitle,
                Title = newBookDetails.Title,
            }, opt =>
            opt.Excluding(f => f.FictionType)
            .Excluding(f => f.FormType)
            .Excluding(f => f.PublicationFormat)
            .Excluding(f => f.Publisher)
            .Excluding(f => f.Authors)
            .Excluding(f => f.Genres));

            var bookEntity = _context.Books.Include(b => b.Authors).Include(b => b.Genres).FirstOrDefault(a => a.BookId == bookGenerated.BookId);

            bookEntity.Genres.Should().HaveCount(2);

            bookEntity.Genres.Should().Contain(new List<Domain.Genre>()
            {
                genre2,
                genre3,
            });

            bookEntity.Authors.Should().HaveCount(2);

            bookEntity.Authors.Should().Contain(new List<Domain.Author>()
            {
                author2,
                author3,
            });
        }
    }
}
