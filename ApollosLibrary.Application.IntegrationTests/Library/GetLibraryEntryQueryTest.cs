using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.IntegrationTests.Generators;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Library.Queries.GetLibraryEntriesQuery;
using ApollosLibrary.Application.Library.Queries.GetLibraryEntryQuery;
using ApollosLibrary.DataLayer.Contracts;
using ApollosLibrary.Domain;
using ApollosLibrary.UnitOfWork.Contracts;
using Bogus;
using FluentAssertions;
using FluentValidation.TestHelper;
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
    public class GetLibraryEntryQueryTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IDateTimeService _dateTimeService;

        public GetLibraryEntryQueryTest(TestFixture fixture) : base(fixture)
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
        public async Task GetLibraryEntries()
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

            var library = new Domain.Library()
            {
                UserId = userID,
                LibraryEntries = new List<LibraryEntry>()
                {
                    entity,
                }
            };

            _context.Libraries.Add(library);
            _context.SaveChanges();

            var command = new GetLibraryEntryQuery()
            { 
                EntryId = entity.EntryId,
            };

            var result = await _mediatr.Send(command);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(new GetLibraryEntryQueryDto()
            {
                BookId = entity.Book.BookId,
                EntryId = entity.EntryId,
                Quantity = entity.Quantity,
            });
        }
    }
}
