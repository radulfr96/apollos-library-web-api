using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyLibrary.Application.Book.Commands.AddBookCommand;
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
    public class AddBookCommandTest : TestBase
    {
        private readonly IDateTimeService _dateTime;
        private readonly MyLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;

        public AddBookCommandTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(new DateTime(2021, 02, 07));
            _dateTime = mockDateTimeService.Object;
            services.AddSingleton(mockDateTimeService.Object);

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<MyLibraryContext>();
            _contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
        }

        [Fact]
        public async Task AddBookCommand()
        {
            var userID = Guid.NewGuid();

            var httpContext = new TestHttpContext();

            httpContext.User = new TestPrincipal(new Claim[]
            {
                new Claim("userid", userID.ToString()),
            });

            _contextAccessor.HttpContext = httpContext;

            var publisher = PublisherGenerator.GetGenericPublisher("AU", userID);
            _context.Publishers.Add(publisher);

            var author1 = AuthorGenerator.GetGenericAuthor(userID, "UK");
            _context.Authors.Add(author1);

            var author2 = AuthorGenerator.GetGenericAuthor(userID, "UK");
            _context.Authors.Add(author2);

            var genre1 = GenreGenerator.GetGenre(userID);
            _context.Genres.Add(genre1);

            var genre2 = GenreGenerator.GetGenre(userID);
            _context.Genres.Add(genre2);

            _context.SaveChanges();

            var bookGenerated = BookGenerator.GetGenericPhysicalBook(userID);

            var command = new AddBookCommand()
            {
                Edition = bookGenerated.Edition,
                FictionTypeID = bookGenerated.FictionTypeId,
                FormTypeID = bookGenerated.FormTypeId,
                ISBN = bookGenerated.Isbn,
                NumberInSeries = bookGenerated.NumberInSeries,
                PublicationFormatID = bookGenerated.PublicationFormatId,
                Subtitle = bookGenerated.Subtitle,
                Title = bookGenerated.Title,
                PublisherID = publisher.PublisherId,
                Genres = new List<int>()
                {
                    genre1.GenreId,
                    genre2.GenreId,
                },
                Authors = new List<int>()
                {
                    author1.AuthorId,
                    author2.AuthorId,
                }
            };

            var result = await _mediatr.Send(command);

            var author = _context.Books.FirstOrDefault(a => a.BookId == bookGenerated.BookId);

            var genres = (from bg in _context.BookGenres
                          join g in _context.Genres
                          on bg.GenreId equals g.GenreId
                          where bg.BookId == result.BookId
                          select g.GenreId).ToList();

            genres.Should().HaveCount(2);


            var authors = (from ba in _context.BookAuthors
                           join a in _context.Authors
                           on ba.AuthorId equals a.AuthorId
                           where ba.BookId == result.BookId
                           select a.AuthorId).ToList();

            authors.Should().HaveCount(2);
        }
    }
}
