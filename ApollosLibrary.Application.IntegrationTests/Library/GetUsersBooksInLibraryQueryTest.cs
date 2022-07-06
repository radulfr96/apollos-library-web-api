using ApollosLibrary.Application.IntegrationTests.Generators;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Library.Commands.CreateLibraryCommand;
using ApollosLibrary.Application.Library.Queries.GetBooksInLibraryQuery;
using ApollosLibrary.Application.Library.Queries.GetUserLibraryIdQuery;
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
    public class GetBooksInLibraryQueryTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IDateTimeService _dateTimeService;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;

        public GetBooksInLibraryQueryTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(LocalDateTime.FromDateTime(new DateTime(2021, 02, 07)));
            services.AddSingleton(mockDateTimeService.Object);
            _dateTimeService = mockDateTimeService.Object;

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContext>();
            _contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
        }

        [Fact]
        public async Task GetUserBooksInLibrary_GetsLibraryBooks()
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

            var book1 = BookGenerator.GetGenericPhysicalBook(userID);
            var book2 = BookGenerator.GetGenericPhysicalBook(userID);

            var entity = new LibraryEntry()
            {
                Book = new Domain.Book()
                {
                    Authors = new List<Domain.Author>()
                            {
                                new Domain.Author()
                                {
                                    FirstName = "Test",
                                    CountryId = "AU",
                                    CreatedBy = userID,
                                    CreatedDate = _dateTimeService.Now,
                                    Description = "Test Desc",
                                    LastName = "Author",
                                }
                            },
                    BookId = book1.BookId,
                    Business = book1.Business,
                    BusinessId = book1.BusinessId,
                    CoverImage = book1.CoverImage,
                    CreatedBy = book1.CreatedBy,
                    CreatedDate = book1.CreatedDate,
                    Edition = book1.Edition,
                    EIsbn = book1.EIsbn,
                    FictionTypeId = book1.FictionTypeId,
                    FormTypeId = book1.FormTypeId,
                    Genres = book1.Genres,
                    Isbn = book1.Isbn,
                    PublicationFormatId = book1.PublicationFormatId,
                    Subtitle = book1.Subtitle,
                    Title = book1.Title,
                },
                Quantity = 2,
            };

            var entity2 = new LibraryEntry()
            {
                Book = new Domain.Book()
                {
                    Authors = new List<Domain.Author>()
                            {
                                new Domain.Author()
                                {
                                    FirstName = "Test",
                                    CountryId = "AU",
                                    CreatedBy = userID,
                                    CreatedDate = _dateTimeService.Now,
                                    Description = "Test Desc",
                                    LastName = "Author",
                                },
                                new Domain.Author()
                                {
                                    FirstName = "Test",
                                    CountryId = "AU",
                                    CreatedBy = userID,
                                    CreatedDate = _dateTimeService.Now,
                                    Description = "Test Desc",
                                    LastName = "Author 2",
                                }
                            },
                    BookId = book2.BookId,
                    Business = book2.Business,
                    BusinessId = book2.BusinessId,
                    CoverImage = book2.CoverImage,
                    CreatedBy = book2.CreatedBy,
                    CreatedDate = book2.CreatedDate,
                    Edition = book2.Edition,
                    EIsbn = book2.EIsbn,
                    FictionTypeId = book2.FictionTypeId,
                    FormTypeId = book2.FormTypeId,
                    Genres = book2.Genres,
                    Isbn = book2.Isbn,
                    PublicationFormatId = book2.PublicationFormatId,
                    Subtitle = book2.Subtitle,
                    Title = book2.Title,
                },
                Quantity = 5,
            };

            var library = new Domain.Library()
            {
                UserId = userID,
                LibraryEntries = new List<LibraryEntry>()
                {
                    entity,
                    entity2
                }
            };

            _context.Libraries.Add(library);
            _context.SaveChanges();

            var query = new GetBooksInLibraryQuery();

            var getBooksResult = await _mediatr.Send(query);

            getBooksResult.Should().NotBeNull();
            getBooksResult.LibraryBooks.Should().HaveCount(2);
            getBooksResult.LibraryBooks.Should().BeEquivalentTo(new List<LibraryBook>()
            {
                new LibraryBook()
                {
                    Author = "Author",
                    BookId = entity.Book.BookId,
                    eISBN = book1.EIsbn,
                    ISBN = book1.Isbn,
                    Title = book1.Title,
                },
                new LibraryBook()
                {
                    Author = "Author et al.",
                    BookId = entity2.Book.BookId,
                    eISBN = book2.EIsbn,
                    ISBN = book2.Isbn,
                    Title = book2.Title,
                }
            });
        }
    }
}
