using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.Book.Commands.AddBookCommand;
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
using ApollosLibrary.Domain.Enums;
using ApollosLibrary.Application.Common.Exceptions;
using NodaTime;

namespace ApollosLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class AddBookCommandTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;

        public AddBookCommandTest(TestFixture fixture) : base(fixture)
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
        public async Task AddBookCommand()
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

            var business = BusinessGenerator.GetGenericBusiness("AU", userID);
            business.BusinessTypeId = (int)BusinessTypeEnum.Publisher;
            _context.Business.Add(business);

            var author1 = AuthorGenerator.GetGenericAuthor(userID, "GB");
            _context.Authors.Add(author1);

            var author2 = AuthorGenerator.GetGenericAuthor(userID, "GB");
            _context.Authors.Add(author2);

            var genre1 = GenreGenerator.GetGenre(userID);
            _context.Genres.Add(genre1);

            var genre2 = GenreGenerator.GetGenre(userID);
            _context.Genres.Add(genre2);

            var series1 = SeriesGenerator.GetSeries(userID);
            _context.Series.Add(series1);

            var series2 = SeriesGenerator.GetSeries(userID);
            _context.Series.Add(series2);

            _context.SaveChanges();

            var bookGenerated = BookGenerator.GetGenericPhysicalBook(userID);

            var command = new AddBookCommand()
            {
                Edition = bookGenerated.Edition,
                FictionTypeId = bookGenerated.FictionTypeId,
                FormTypeId = bookGenerated.FormTypeId,
                ISBN = bookGenerated.Isbn,
                PublicationFormatId = bookGenerated.PublicationFormatId,
                Subtitle = bookGenerated.Subtitle,
                Title = bookGenerated.Title,
                BusinessId = business.BusinessId,
                Genres = new List<int>()
                {
                    genre1.GenreId,
                    genre2.GenreId,
                },
                Authors = new List<int>()
                {
                    author1.AuthorId,
                    author2.AuthorId,
                },
                Series = new List<int>()
                {
                    series1.SeriesId,
                    series2.SeriesId,
                },
            };

            var result = await _mediatr.Send(command);

            var book = _context.Books
                                .Include(b => b.Genres)
                                .Include(b => b.Authors)
                                .Include(b => b.Series)
                                .FirstOrDefault(a => a.BookId == result.BookId);

            book.Genres.Should().HaveCount(2);

            book.Authors.Should().HaveCount(2);

            book.Series.Should().HaveCount(2);
        }

        [Fact]
        public async Task AddExistingDeletedISBN_UpdateBook()
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

            var bookGenerated = BookGenerator.GetGenericPhysicalBook(userID);
            bookGenerated.IsDeleted = true;
            var bookGenerated2 = BookGenerator.GetGenericPhysicalBook(userID);

            _context.Books.Add(bookGenerated);

            var command = new AddBookCommand()
            {
                Edition = bookGenerated2.Edition,
                FictionTypeId = bookGenerated2.FictionTypeId,
                FormTypeId = bookGenerated2.FormTypeId,
                ISBN = bookGenerated.Isbn,
                PublicationFormatId = bookGenerated2.PublicationFormatId,
                Subtitle = bookGenerated2.Subtitle,
                Title = bookGenerated2.Title,
            };

            _context.SaveChanges();

            var result = await _mediatr.Send(command);

            result.BookId.Should().Be(bookGenerated.BookId);

            var updatedBook = _context.Books.FirstOrDefault(b => b.BookId == result.BookId);

            updatedBook.Isbn.Should().BeEquivalentTo(command.ISBN);
            updatedBook.Title.Should().BeEquivalentTo(command.Title);
            updatedBook.Subtitle.Should().BeEquivalentTo(command.Subtitle);
            updatedBook.Edition.Should().Be(command.Edition);
            updatedBook.IsDeleted.Should().BeFalse();
        }

        [Fact]
        public async Task AddExistingDeletedEISBN_UpdateBook()
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

            var bookGenerated = BookGenerator.GetGenericDigitalBook(userID);
            bookGenerated.IsDeleted = true;
            var bookGenerated2 = BookGenerator.GetGenericDigitalBook(userID);

            _context.Books.Add(bookGenerated);

            var command = new AddBookCommand()
            {
                Edition = bookGenerated2.Edition,
                FictionTypeId = bookGenerated2.FictionTypeId,
                FormTypeId = bookGenerated2.FormTypeId,
                EISBN = bookGenerated.EIsbn,
                PublicationFormatId = bookGenerated2.PublicationFormatId,
                Subtitle = bookGenerated2.Subtitle,
                Title = bookGenerated2.Title,
            };

            _context.SaveChanges();

            var result = await _mediatr.Send(command);

            result.BookId.Should().Be(bookGenerated.BookId);

            var updatedBook = _context.Books.FirstOrDefault(b => b.BookId == result.BookId);

            updatedBook.EIsbn.Should().BeEquivalentTo(command.EISBN);
            updatedBook.Title.Should().BeEquivalentTo(command.Title);
            updatedBook.Subtitle.Should().BeEquivalentTo(command.Subtitle);
            updatedBook.Edition.Should().Be(command.Edition);
            updatedBook.IsDeleted.Should().BeFalse();
        }
    }
}
