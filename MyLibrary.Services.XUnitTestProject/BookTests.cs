using Moq;
using MyLibrary.Data.Model;
using MyLibrary.DataLayer.Contracts;
using MyLibrary.UnitOfWork;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace MyLibrary.Services.XUnitTestProject
{
    public class BookTests
    {
        private readonly ClaimsPrincipal MockPrincipal;

        public BookTests()
        {
            MockPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Sid, "1")
            });
        }

        [Fact]
        public void GetBookFailNotFound()
        {
            var dataLayer = new Mock<IBookDataLayer>();
            var uow = new Mock<IBookUnitOfWork>();

            uow.Setup(b => b.BookDataLayer).Returns(dataLayer.Object);

            var service = new BookService(uow.Object, MockPrincipal);

            var response = service.GetBook(1);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public void GetBookSuccess()
        {
            var dataLayer = new Mock<IBookDataLayer>();
            var uow = new Mock<IBookUnitOfWork>();

            dataLayer.Setup(b => b.GetBook(It.IsAny<int>())).Returns(new Book()
            {
                CreatedDate = DateTime.Now,
                BookId = 1,
                CoverImage = null,
                CreatedBy = 1,
                Edition = 1,
                EIsbn = "465564654646546",
                FictionTypeId = 1,
                FormTypeId = 1,
                NumberInSeries = 1,
                PublicationFormatId = 1,
                PublisherId = 1,
                SeriesId = 1,
                Subtitle = "Book For Testing",
                Title = "Test Book",
                BookAuthor = new List<BookAuthor>()
                {
                    new BookAuthor()
                    {
                        AuthorId = 1,
                        BookId = 1
                    }
                },
                BookGenre = new List<BookGenre>()
                {
                    new BookGenre()
                    {
                        GenreId = 1,
                        BookId = 1,
                    }
                }
            });
            uow.Setup(b => b.BookDataLayer).Returns(dataLayer.Object);

            var service = new BookService(uow.Object, MockPrincipal);

            var response = service.GetBook(1);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, response.Book.BookID);
            Assert.True(response.Book.Authors.Count == 1);
            Assert.True(response.Book.Genres.Count == 1);
            Assert.Equal(1, response.Book.Edition);
            Assert.Equal("465564654646546", response.Book.eISBN);
            Assert.Equal(1, response.Book.FictionType);
            Assert.Equal(1, response.Book.FormType);
            Assert.Equal(1, response.Book.NumberInSeries);
            Assert.Equal(1, response.Book.PublicationFormat);
            Assert.Equal(1, response.Book.Publisher);
            Assert.Equal(1, response.Book.SeriesID);
            Assert.Equal("Book For Testing", response.Book.Subtitle);
            Assert.Equal("Test Book", response.Book.Title);
        }
    }
}
