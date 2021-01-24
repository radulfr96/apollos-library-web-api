using Moq;
using MyLibrary.DataLayer.Contracts;
using MyLibrary.Persistence.Model;
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
        //private readonly ClaimsPrincipal MockPrincipal;

        //public BookTests()
        //{
        //    MockPrincipal = new TestPrincipal(new Claim[]
        //    {
        //        new Claim(ClaimTypes.Name, "Test User"),
        //        new Claim(ClaimTypes.Sid, "1")
        //    });
        //}

        //[Fact]
        //public void AddBookFailNoTitle()
        //{
        //    var dataLayer = new Mock<IBookDataLayer>();
        //    var uow = new Mock<IBookUnitOfWork>();

        //    dataLayer.Setup(b => b.AddBook(It.IsAny<Book>())).Callback((Book book) =>
        //    {
        //        book.BookId = 1;
        //    });

        //    uow.Setup(b => b.BookDataLayer).Returns(dataLayer.Object);

        //    var service = new BookService(uow.Object, MockPrincipal);

        //    var request = new AddBookRequest();

        //    var response = service.AddBook(request);

        //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //    Assert.Equal("You must provide a title", response.Messages[0]);

        //    request.Title = "";

        //    response = service.AddBook(request);

        //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //    Assert.Equal("You must provide a title", response.Messages[0]);
        //}

        //[Fact]
        //public void AddBookFailNoISBNOrEISBN()
        //{
        //    var dataLayer = new Mock<IBookDataLayer>();
        //    var uow = new Mock<IBookUnitOfWork>();

        //    dataLayer.Setup(b => b.AddBook(It.IsAny<Book>())).Callback((Book book) =>
        //    {
        //        book.BookId = 1;
        //    });

        //    uow.Setup(b => b.BookDataLayer).Returns(dataLayer.Object);

        //    var service = new BookService(uow.Object, MockPrincipal);

        //    var request = new AddBookRequest()
        //    {
        //        Title = "Test title"
        //    };

        //    var response = service.AddBook(request);

        //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //    Assert.Equal("You must provide a ISBN or an eISBN", response.Messages[0]);

        //    request.ISBN = "";
        //    request.eISBN = "";

        //    response = service.AddBook(request);

        //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //    Assert.Equal("You must provide a ISBN or an eISBN", response.Messages[0]);
        //}

        //[Fact]
        //public void AddBookFailDuplicateISBN()
        //{
        //    var dataLayer = new Mock<IBookDataLayer>();
        //    var uow = new Mock<IBookUnitOfWork>();

        //    dataLayer.Setup(b => b.GetBookByISBN(It.IsAny<string>())).Returns(new Book() { });

        //    uow.Setup(b => b.BookDataLayer).Returns(dataLayer.Object);

        //    var service = new BookService(uow.Object, MockPrincipal);

        //    var request = new AddBookRequest()
        //    {
        //        ISBN = "464654646",
        //        Title = "Test Title"
        //    };

        //    var response = service.AddBook(request);

        //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //    Assert.Equal("Book with that ISBN already exists.", response.Messages[0]);
        //}

        //[Fact]
        //public void AddBookFailDuplicateEISBN()
        //{
        //    var dataLayer = new Mock<IBookDataLayer>();
        //    var uow = new Mock<IBookUnitOfWork>();

        //    dataLayer.Setup(b => b.GetBookByeISBN(It.IsAny<string>())).Returns(new Book() { });

        //    uow.Setup(b => b.BookDataLayer).Returns(dataLayer.Object);

        //    var service = new BookService(uow.Object, MockPrincipal);

        //    var request = new AddBookRequest()
        //    {
        //        eISBN = "464654646",
        //        Title = "Test Title"
        //    };

        //    var response = service.AddBook(request);

        //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //    Assert.Equal("Book with that eISBN already exists.", response.Messages[0]);
        //}

        //[Fact]
        //public void AddBookSuccessMinDataEISBN()
        //{
        //    var books = new List<Book>();

        //    var dataLayer = new Mock<IBookDataLayer>();
        //    var uow = new Mock<IBookUnitOfWork>();

        //    dataLayer.Setup(b => b.AddBook(It.IsAny<Book>())).Callback((Book book) =>
        //    {
        //        book.BookId = 1;
        //        books.Add(book);
        //    });

        //    uow.Setup(b => b.BookDataLayer).Returns(dataLayer.Object);

        //    var service = new BookService(uow.Object, MockPrincipal);

        //    var request = new AddBookRequest()
        //    {
        //        Title = "Test title",
        //        eISBN = "4564654646646"
        //    };

        //    var response = service.AddBook(request);

        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //    Assert.Equal(1, response.BookID);
        //    Assert.Equal(1, books[0].BookId);
        //    Assert.Equal("4564654646646", books[0].EIsbn);
        //    Assert.Equal("Test title", books[0].Title);
        //}

        //[Fact]
        //public void AddBookSuccessMinDataISBN()
        //{
        //    var books = new List<Book>();

        //    var dataLayer = new Mock<IBookDataLayer>();
        //    var uow = new Mock<IBookUnitOfWork>();

        //    dataLayer.Setup(b => b.AddBook(It.IsAny<Book>())).Callback((Book book) =>
        //    {
        //        book.BookId = 1;
        //        books.Add(book);
        //    });

        //    uow.Setup(b => b.BookDataLayer).Returns(dataLayer.Object);

        //    var service = new BookService(uow.Object, MockPrincipal);

        //    var request = new AddBookRequest()
        //    {
        //        Title = "Test title",
        //        ISBN = "554156566",
        //    };

        //    var response = service.AddBook(request);

        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //    Assert.Equal(1, response.BookID);
        //    Assert.Equal(1, books[0].BookId);
        //    Assert.Equal("554156566", books[0].Isbn);
        //    Assert.Equal("Test title", books[0].Title);
        //}

        //[Fact]
        //public void AddBookSuccess()
        //{
        //    var books = new List<Book>();
        //    var bookAuthors = new List<BookAuthor>();
        //    var bookGenres = new List<BookGenre>();

        //    var dataLayer = new Mock<IBookDataLayer>();
        //    var uow = new Mock<IBookUnitOfWork>();

        //    dataLayer.Setup(b => b.AddBook(It.IsAny<Book>())).Callback((Book book) =>
        //    {
        //        book.BookId = 1;
        //        books.Add(book);
        //    });

        //    dataLayer.Setup(b => b.AddBookAuthor(It.IsAny<BookAuthor>())).Callback((BookAuthor bookAuthor) =>
        //    {
        //        bookAuthors.Add(bookAuthor);
        //    });

        //    dataLayer.Setup(b => b.AddBookGenre(It.IsAny<BookGenre>())).Callback((BookGenre bookGenre) =>
        //    {
        //        bookGenres.Add(bookGenre);
        //    });

        //    uow.Setup(b => b.BookDataLayer).Returns(dataLayer.Object);

        //    var service = new BookService(uow.Object, MockPrincipal);

        //    var request = new AddBookRequest()
        //    {
        //        Title = "Test title",
        //        ISBN = "554156566",
        //        Authors = new List<int>()
        //        {
        //            2,
        //        },
        //        CoverImage = new byte[100],
        //        Edition = 1,
        //        eISBN = "4646464660",
        //        FictionTypeID = 1,
        //        FormTypeID = 4,
        //        Genres = new List<int>()
        //        {
        //            4,
        //        },
        //        PublicationFormatID = 1,
        //        NumberInSeries = 2,
        //        PublisherIDs = 9,
        //        SeriesID = 85,
        //        Subtitle = "Test Subtitle"
        //    };

        //    var response = service.AddBook(request);

        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //    Assert.Equal(1, response.BookID);
        //    Assert.Equal(1, books[0].BookId);
        //    Assert.Equal("554156566", books[0].Isbn);
        //    Assert.Equal("Test title", books[0].Title);
        //    Assert.Equal(2, bookAuthors[0].AuthorId);
        //    Assert.Equal(1, bookAuthors[0].BookId);
        //    Assert.Equal(1, books[0].Edition);
        //    Assert.Equal("4646464660", books[0].EIsbn);
        //    Assert.Equal(1, books[0].FictionTypeId);
        //    Assert.Equal(4, books[0].FormTypeId);
        //    Assert.Equal(2, books[0].NumberInSeries);
        //    Assert.Equal(1, books[0].PublicationFormatId);
        //    Assert.Equal(85, books[0].SeriesId);
        //    Assert.Equal(9, books[0].PublisherId);
        //    Assert.Equal("Test Subtitle", books[0].Subtitle);
        //}

        //[Fact]
        //public void GetBooksFailNotFound()
        //{
        //    var dataLayer = new Mock<IBookDataLayer>();
        //    var uow = new Mock<IBookUnitOfWork>();

        //    uow.Setup(b => b.BookDataLayer).Returns(dataLayer.Object);

        //    var service = new BookService(uow.Object, MockPrincipal);

        //    var response = service.GetBooks();

        //    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        //}

        //[Fact]
        //public void GetBooksSuccess()
        //{
        //    var dataLayer = new Mock<IBookDataLayer>();
        //    var uow = new Mock<IBookUnitOfWork>();

        //    dataLayer.Setup(b => b.GetBooks()).Returns(new List<Book>()
        //    {
        //        new Book()
        //        {
        //            CreatedDate = DateTime.Now,
        //            BookId = 1,
        //            CoverImage = null,
        //            CreatedBy = 1,
        //            Edition = 1,
        //            EIsbn = "465564654646546",
        //            FictionTypeId = 1,
        //            FictionType = new FictionType()
        //            {
        //                TypeId = 1,
        //                Name = "Test Fiction Type",
        //            },
        //            FormTypeId = 1,
        //            NumberInSeries = 1,
        //            PublicationFormatId = 1,
        //            PublicationFormat = new PublicationFormat()
        //            {
        //                TypeId = 1,
        //                Name = "Test Publication Format"
        //            },
        //            FormType = new FormType()
        //            {
        //                TypeId = 1,
        //                Name = "Test Format Type"
        //            },
        //            PublisherId = 1,
        //            SeriesId = 1,
        //            Subtitle = "Book For Testing",
        //            Title = "Test Book",
        //            BookAuthor = new List<BookAuthor>()
        //            {
        //                new BookAuthor()
        //                {
        //                    AuthorId = 1,
        //                    BookId = 1
        //                }
        //            },
        //            BookGenre = new List<BookGenre>()
        //            {
        //                new BookGenre()
        //                {
        //                    GenreId = 1,
        //                    BookId = 1,
        //                }
        //            }
        //        }
        //    });
        //    uow.Setup(b => b.BookDataLayer).Returns(dataLayer.Object);

        //    var service = new BookService(uow.Object, MockPrincipal);

        //    var response = service.GetBooks();

        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //    Assert.Equal(1, response.Books[0].BookID);
        //    Assert.Equal("465564654646546", response.Books[0].eISBN);
        //    Assert.Equal("Test Fiction Type", response.Books[0].FictionType);
        //    Assert.Equal("Test Format Type", response.Books[0].FormatType);
        //    Assert.Equal("Test Book", response.Books[0].Title);
        //}

        //[Fact]
        //public void GetBookFailNotFound()
        //{
        //    var dataLayer = new Mock<IBookDataLayer>();
        //    var uow = new Mock<IBookUnitOfWork>();

        //    uow.Setup(b => b.BookDataLayer).Returns(dataLayer.Object);

        //    var service = new BookService(uow.Object, MockPrincipal);

        //    var response = service.GetBook(1);

        //    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        //}

        //[Fact]
        //public void GetBookSuccess()
        //{
        //    var dataLayer = new Mock<IBookDataLayer>();
        //    var uow = new Mock<IBookUnitOfWork>();

        //    dataLayer.Setup(b => b.GetBook(It.IsAny<int>())).Returns(new Book()
        //    {
        //        CreatedDate = DateTime.Now,
        //        BookId = 1,
        //        CoverImage = null,
        //        CreatedBy = 1,
        //        Edition = 1,
        //        EIsbn = "465564654646546",
        //        FictionTypeId = 1,
        //        FormTypeId = 1,
        //        NumberInSeries = 1,
        //        PublicationFormatId = 1,
        //        PublisherId = 1,
        //        SeriesId = 1,
        //        Subtitle = "Book For Testing",
        //        Title = "Test Book",
        //        BookAuthor = new List<BookAuthor>()
        //        {
        //            new BookAuthor()
        //            {
        //                AuthorId = 1,
        //                BookId = 1
        //            }
        //        },
        //        BookGenre = new List<BookGenre>()
        //        {
        //            new BookGenre()
        //            {
        //                GenreId = 1,
        //                BookId = 1,
        //            }
        //        }
        //    });
        //    uow.Setup(b => b.BookDataLayer).Returns(dataLayer.Object);

        //    var service = new BookService(uow.Object, MockPrincipal);

        //    var response = service.GetBook(1);

        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //    Assert.Equal(1, response.Book.BookID);
        //    Assert.True(response.Book.Authors.Count == 1);
        //    Assert.True(response.Book.Genres.Count == 1);
        //    Assert.Equal(1, response.Book.Edition);
        //    Assert.Equal("465564654646546", response.Book.eISBN);
        //    Assert.Equal(1, response.Book.FictionType);
        //    Assert.Equal(1, response.Book.FormType);
        //    Assert.Equal(1, response.Book.NumberInSeries);
        //    Assert.Equal(1, response.Book.PublicationFormat);
        //    Assert.Equal(1, response.Book.Publisher);
        //    Assert.Equal(1, response.Book.SeriesID);
        //    Assert.Equal("Book For Testing", response.Book.Subtitle);
        //    Assert.Equal("Test Book", response.Book.Title);
        //}

        //[Fact]
        //public void UpdateBookFailNotFound()
        //{
        //    var dataLayer = new Mock<IBookDataLayer>();
        //    var uow = new Mock<IBookUnitOfWork>();

        //    dataLayer.Setup(b => b.GetBook(It.IsAny<int>())).Returns((Book)null);

        //    uow.Setup(b => b.BookDataLayer).Returns(dataLayer.Object);

        //    var service = new BookService(uow.Object, MockPrincipal);

        //    var request = new UpdateBookRequest()
        //    {
        //        BookID = 1,
        //        Title = "Test Title",
        //        ISBN = "41546546"
        //    };

        //    var response = service.UpdateBook(request);

        //    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        //}

        //[Fact]
        //public void UpdateBookFailNoTitle()
        //{
        //    var dataLayer = new Mock<IBookDataLayer>();
        //    var uow = new Mock<IBookUnitOfWork>();

        //    dataLayer.Setup(b => b.GetBook(It.IsAny<int>())).Returns(new Book());

        //    uow.Setup(b => b.BookDataLayer).Returns(dataLayer.Object);

        //    var service = new BookService(uow.Object, MockPrincipal);

        //    var request = new UpdateBookRequest();

        //    var response = service.UpdateBook(request);

        //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //    Assert.Equal("You must provide a title", response.Messages[0]);

        //    request.Title = "";

        //    response = service.UpdateBook(request);

        //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //    Assert.Equal("You must provide a title", response.Messages[0]);
        //}

        //[Fact]
        //public void UpdateBookFailNoISBNOrEISBN()
        //{
        //    var dataLayer = new Mock<IBookDataLayer>();
        //    var uow = new Mock<IBookUnitOfWork>();

        //    dataLayer.Setup(b => b.GetBook(It.IsAny<int>())).Returns(new Book());

        //    uow.Setup(b => b.BookDataLayer).Returns(dataLayer.Object);

        //    var service = new BookService(uow.Object, MockPrincipal);

        //    var request = new UpdateBookRequest()
        //    {
        //        Title = "Test title"
        //    };

        //    var response = service.UpdateBook(request);

        //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //    Assert.Equal("You must provide a ISBN or an eISBN", response.Messages[0]);

        //    request.ISBN = "";
        //    request.eISBN = "";

        //    response = service.UpdateBook(request);

        //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //    Assert.Equal("You must provide a ISBN or an eISBN", response.Messages[0]);
        //}

        //[Fact]
        //public void UpdateBookFailDuplicateISBN()
        //{
        //    var dataLayer = new Mock<IBookDataLayer>();
        //    var uow = new Mock<IBookUnitOfWork>();

        //    dataLayer.Setup(b => b.GetBook(It.IsAny<int>())).Returns(new Book());
        //    dataLayer.Setup(b => b.GetBookByISBN(It.IsAny<string>())).Returns(new Book() { });

        //    uow.Setup(b => b.BookDataLayer).Returns(dataLayer.Object);

        //    var service = new BookService(uow.Object, MockPrincipal);

        //    var request = new UpdateBookRequest()
        //    {
        //        ISBN = "464654646",
        //        Title = "Test Title"
        //    };

        //    var response = service.UpdateBook(request);

        //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //    Assert.Equal("Book with that ISBN already exists.", response.Messages[0]);
        //}

        //[Fact]
        //public void UpdateBookFailDuplicateEISBN()
        //{
        //    var dataLayer = new Mock<IBookDataLayer>();
        //    var uow = new Mock<IBookUnitOfWork>();

        //    dataLayer.Setup(b => b.GetBook(It.IsAny<int>())).Returns(new Book());
        //    dataLayer.Setup(b => b.GetBookByeISBN(It.IsAny<string>())).Returns(new Book() { });

        //    uow.Setup(b => b.BookDataLayer).Returns(dataLayer.Object);

        //    var service = new BookService(uow.Object, MockPrincipal);

        //    var request = new UpdateBookRequest()
        //    {
        //        eISBN = "464654646",
        //        Title = "Test Title"
        //    };

        //    var response = service.UpdateBook(request);

        //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //    Assert.Equal("Book with that eISBN already exists.", response.Messages[0]);
        //}

        //[Fact]
        //public void UpdateBookSuccessMinDataEISBN()
        //{
        //    var book = new Book()
        //    {
        //        BookId = 1,
        //    };

        //    var dataLayer = new Mock<IBookDataLayer>();
        //    var uow = new Mock<IBookUnitOfWork>();

        //    dataLayer.Setup(b => b.GetBook(It.IsAny<int>())).Returns(book);

        //    uow.Setup(b => b.BookDataLayer).Returns(dataLayer.Object);

        //    var service = new BookService(uow.Object, MockPrincipal);

        //    var request = new UpdateBookRequest()
        //    {
        //        Title = "Test title",
        //        eISBN = "4564654646646"
        //    };

        //    var response = service.UpdateBook(request);

        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //    Assert.Equal(1, book.BookId);
        //    Assert.Equal("4564654646646", book.EIsbn);
        //    Assert.Equal("Test title", book.Title);
        //}

        //[Fact]
        //public void UpdateBookSuccessMinDataISBN()
        //{
        //    var book = new Book()
        //    {
        //        BookId = 1,
        //    };

        //    var dataLayer = new Mock<IBookDataLayer>();
        //    var uow = new Mock<IBookUnitOfWork>();

        //    dataLayer.Setup(b => b.GetBook(It.IsAny<int>())).Returns(book);

        //    uow.Setup(b => b.BookDataLayer).Returns(dataLayer.Object);

        //    var service = new BookService(uow.Object, MockPrincipal);

        //    var request = new UpdateBookRequest()
        //    {
        //        Title = "Test title",
        //        ISBN = "554156566",
        //    };

        //    var response = service.UpdateBook(request);

        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //    Assert.Equal(1, book.BookId);
        //    Assert.Equal("554156566", book.Isbn);
        //    Assert.Equal("Test title", book.Title);
        //}

        //[Fact]
        //public void UpdateBookSuccess()
        //{
        //    var book = new Book()
        //    {
        //        BookId = 1
        //    };

        //    var bookAuthors = new List<BookAuthor>();
        //    var bookGenres = new List<BookGenre>();

        //    var dataLayer = new Mock<IBookDataLayer>();
        //    var uow = new Mock<IBookUnitOfWork>();

        //    dataLayer.Setup(b => b.GetBook(It.IsAny<int>())).Returns(book);

        //    dataLayer.Setup(b => b.AddBookAuthor(It.IsAny<BookAuthor>())).Callback((BookAuthor bookAuthor) =>
        //    {
        //        bookAuthors.Add(bookAuthor);
        //    });

        //    dataLayer.Setup(b => b.AddBookGenre(It.IsAny<BookGenre>())).Callback((BookGenre bookGenre) =>
        //    {
        //        bookGenres.Add(bookGenre);
        //    });

        //    uow.Setup(b => b.BookDataLayer).Returns(dataLayer.Object);

        //    var service = new BookService(uow.Object, MockPrincipal);

        //    var request = new UpdateBookRequest()
        //    {
        //        Title = "Test title",
        //        ISBN = "554156566",
        //        Authors = new List<int>()
        //        {
        //            2,
        //        },
        //        CoverImage = new byte[100],
        //        Edition = 1,
        //        eISBN = "4646464660",
        //        FictionTypeID = 1,
        //        FormTypeID = 4,
        //        Genres = new List<int>()
        //        {
        //            4,
        //        },
        //        PublicationFormatID = 1,
        //        NumberInSeries = 2,
        //        PublisherIDs = 9,
        //        SeriesID = 85,
        //        Subtitle = "Test Subtitle"
        //    };

        //    var response = service.UpdateBook(request);

        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //    Assert.Equal(1, book.BookId);
        //    Assert.Equal("554156566", book.Isbn);
        //    Assert.Equal("Test title", book.Title);
        //    Assert.Equal(2, bookAuthors[0].AuthorId);
        //    Assert.Equal(1, bookAuthors[0].BookId);
        //    Assert.Equal(1, book.Edition);
        //    Assert.Equal("4646464660", book.EIsbn);
        //    Assert.Equal(1, book.FictionTypeId);
        //    Assert.Equal(4, book.FormTypeId);
        //    Assert.Equal(2, book.NumberInSeries);
        //    Assert.Equal(1, book.PublicationFormatId);
        //    Assert.Equal(85, book.SeriesId);
        //    Assert.Equal(9, book.PublisherId);
        //    Assert.Equal("Test Subtitle", book.Subtitle);
        //}
    }
}
