using Microsoft.Extensions.Logging;
using MyLibrary.Common.DTOs;
using MyLibrary.Common.Responses;
using MyLibrary.Data.Model;
using MyLibrary.Services.Contracts;
using MyLibrary.UnitOfWork.Contracts;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace MyLibrary.Services
{
    public class BookService : IBookService
    {
        private readonly IBookUnitOfWork _bookUnitOfWork;
        private readonly ClaimsPrincipal _principal;

        protected static Logger s_logger = LogManager.GetCurrentClassLogger();

        public BookService(IBookUnitOfWork bookUnitOfWork, ClaimsPrincipal principal)
        {
            _bookUnitOfWork = bookUnitOfWork;
            _principal = principal;
        }

        public GetBookResponse GetBook(int id)
        {
            var response = new GetBookResponse();
            try
            {
                var book = _bookUnitOfWork.BookDataLayer.GetBook(id);

                if (book == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }

                response.Book = DAO2DTO(book);
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to find book.");
                response = new GetBookResponse();
            }
            return response;
        }

        public GetBooksResponse GetBooks()
        {
            var response = new GetBooksResponse();
            try
            {
                var books = _bookUnitOfWork.BookDataLayer.GetBooks();

                if (books == null || books.Count == 0)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }

                response.Books = books.Select(b => new BookListItemDTO()
                {
                    BookID = b.BookId,
                    eISBN = b.EIsbn,
                    FictionType = b.FictionType.Name,
                    ISBN = b.Isbn,
                    FormatType = b.FormType.Name,
                    Title = b.Title
                }).ToList();
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to find books.");
                response = new GetBooksResponse();
            }
            return response;
        }

        public BookDTO DAO2DTO(Book book)
        {
            return new BookDTO()
            {
                BookID = book.BookId,
                CoverImage = book.CoverImage == null ? null : Encoding.ASCII.GetBytes(book.CoverImage),
                Edition = book.Edition,
                eISBN = book.EIsbn,
                FictionType = book.FictionTypeId,
                FormType = book.FormTypeId,
                ISBN = book.Isbn,
                NumberInSeries = book.NumberInSeries,
                PublicationFormat = book.PublicationFormatId,
                Publisher = book.PublisherId,
                SeriesID = book.SeriesId,
                Subtitle = book.Subtitle,
                Title = book.Title,
                Authors = book.BookAuthor.Select(ba => ba.AuthorId).ToList(),
                Genres = book.BookGenre.Select(bg => bg.GenreId).ToList(),
            };
        }
    }
}
