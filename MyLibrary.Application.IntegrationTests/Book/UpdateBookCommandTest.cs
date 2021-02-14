using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyLibrary.Application.Book.Commands.UpdateBookCommand;
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

namespace MyLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class UpdateBookCommandTest : TestBase
    {
        private readonly IDateTimeService _dateTime;
        private readonly MyLibraryContext _context;
        private readonly IMediator _mediatr;

        public UpdateBookCommandTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(new DateTime(2021, 02, 07));
            _dateTime = mockDateTimeService.Object;
            services.AddSingleton(mockDateTimeService.Object);

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<MyLibraryContext>();
        }

        [Fact]
        public async Task UpdateBookCommand()
        {
            Thread.CurrentPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Sid, "1"),
            });

            var publisher1 = PublisherGenerator.GetGenericPublisher("UK", 1);
            _context.Publishers.Add(publisher1);

            var publisher2 = PublisherGenerator.GetGenericPublisher("UK", 1);
            _context.Publishers.Add(publisher2);

            _context.SaveChanges();

            var author1 = AuthorGenerator.GetGenericAuthor(1, "UK");
            _context.Authors.Add(author1);

            var author2 = AuthorGenerator.GetGenericAuthor(1, "UK");
            _context.Authors.Add(author2);

            var author3 = AuthorGenerator.GetGenericAuthor(1, "UK");
            _context.Authors.Add(author3);

            var genre1 = GenreGenerator.GetGenre(1);
            _context.Genres.Add(genre1);

            var genre2 = GenreGenerator.GetGenre(1);
            _context.Genres.Add(genre2);

            var genre3 = GenreGenerator.GetGenre(1);
            _context.Genres.Add(genre3);

            _context.SaveChanges();

            var bookGenerated = BookGenerator.GetGenericPhysicalBook(1);
            bookGenerated.PublisherId = publisher1.PublisherId;

            _context.Books.Add(bookGenerated);

            _context.SaveChanges();

            _context.BookGenres.Add(new BookGenre()
            {
                BookId = bookGenerated.BookId,
                GenreId = genre1.GenreId,
            });

            _context.BookGenres.Add(new BookGenre()
            {
                BookId = bookGenerated.BookId,
                GenreId = genre2.GenreId,
            });

            _context.BookAuthors.Add(new BookAuthor()
            {
                BookId = bookGenerated.BookId,
                AuthorId = author1.AuthorId,
            });

            _context.BookAuthors.Add(new BookAuthor()
            {
                BookId = bookGenerated.BookId,
                AuthorId = author2.AuthorId,
            });

            var newBookDetails = BookGenerator.GetGenericPhysicalBook(1);

            var command = new UpdateBookCommand()
            {
                Authors = new List<int>()
                {
                    author2.AuthorId,
                    author3.AuthorId,
                },
                BookID = bookGenerated.BookId,
                Edition = newBookDetails.Edition,
                EISBN = newBookDetails.EIsbn,
                FictionTypeID = newBookDetails.FictionTypeId,
                FormTypeID = newBookDetails.FormTypeId,
                Genres = new List<int>()
                {
                    genre2.GenreId,
                    genre3.GenreId,
                },
                ISBN = newBookDetails.Isbn,
                NumberInSeries = newBookDetails.NumberInSeries,
                PublicationFormatID = newBookDetails.PublicationFormatId,
                PublisherID = publisher2.PublisherId,
                SeriesID = newBookDetails.SeriesId,
                Subtitle = newBookDetails.Subtitle,
                Title = newBookDetails.Title,
            };

            var result = await _mediatr.Send(command);

            var book = _context.Books.FirstOrDefault(b => b.BookId == bookGenerated.BookId);

            book.Should().BeEquivalentTo(new Persistence.Model.Book()
            {
                BookId = bookGenerated.BookId,
                CreatedBy = 1,
                CreatedDate = bookGenerated.CreatedDate,
                Edition = newBookDetails.Edition,
                EIsbn = newBookDetails.EIsbn,
                FictionTypeId = newBookDetails.FictionTypeId,
                FormTypeId = newBookDetails.FormTypeId,
                Isbn = newBookDetails.Isbn,
                ModifiedBy = 1,
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
            .Excluding(f => f.BookAuthors)
            .Excluding(f => f.BookGenres));

            var author = _context.Books.FirstOrDefault(a => a.BookId == bookGenerated.BookId);

            var genres = (from bg in _context.BookGenres
                          join g in _context.Genres
                          on bg.GenreId equals g.GenreId
                          where bg.BookId == bookGenerated.BookId
                          select g.GenreId).ToList();

            genres.Should().HaveCount(2);

            genres.Should().Contain(new List<int>()
            {
                genre2.GenreId,
                genre3.GenreId,
            });

            var authors = (from ba in _context.BookAuthors
                           join a in _context.Authors
                           on ba.AuthorId equals a.AuthorId
                           where ba.BookId == bookGenerated.BookId
                           select a.AuthorId).ToList();

            authors.Should().HaveCount(2);

            authors.Should().Contain(new List<int>()
            {
                author2.AuthorId,
                author3.AuthorId,
            });
        }
    }
}
