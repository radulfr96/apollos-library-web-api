using Microsoft.Extensions.Logging;
using MyLibrary.Common.DTOs;
using MyLibrary.Common.Requests;
using MyLibrary.Common.Requests.Book;
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

        public AddBookResponse AddBook(AddBookRequest request)
        {
            var response = new AddBookResponse();

            try
            {
                response = (AddBookResponse)request.ValidateRequest(response);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    return response;

                if (!string.IsNullOrEmpty(request.eISBN))
                {
                    var existingISBN = _bookUnitOfWork.BookDataLayer.GetBookByeISBN(request.eISBN);

                    if (existingISBN != null)
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.Messages.Add("Book with that eISBN already exists.");
                        return response;
                    }

                }

                if (!string.IsNullOrEmpty(request.ISBN))
                {
                    var existingeISBN = _bookUnitOfWork.BookDataLayer.GetBookByISBN(request.ISBN);

                    if (existingeISBN != null)
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.Messages.Add("Book with that ISBN already exists.");
                        return response;
                    }

                }

                var book = new Book()
                {
                    CoverImage = request.CoverImage == null ? null : Convert.ToBase64String(request.CoverImage),
                    CreatedBy = int.Parse(_principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value),
                    CreatedDate = DateTime.Now,
                    Edition = request.Edition,
                    EIsbn = request.eISBN,
                    FictionTypeId = request.FictionTypeID,
                    FormTypeId = request.FormTypeID,
                    Isbn = request.ISBN,
                    NumberInSeries = request.NumberInSeries,
                    PublicationFormatId = request.PublicationFormatID,
                    PublisherId = request.PublisherIDs,
                    SeriesId = request.SeriesID,
                    Subtitle = request.Subtitle,
                    Title = request.Title,
                };

                _bookUnitOfWork.Begin();

                _bookUnitOfWork.BookDataLayer.AddBook(book);

                foreach (int authorId in request.Authors)
                {
                    _bookUnitOfWork.BookDataLayer.AddBookAuthor(new BookAuthor()
                    {
                        AuthorId = authorId,
                        BookId = book.BookId,
                    });
                }

                foreach (int genreId in request.Genres)
                {
                    _bookUnitOfWork.BookDataLayer.AddBookGenre(new BookGenre()
                    {
                        GenreId = genreId,
                        BookId = book.BookId,
                    });
                }

                _bookUnitOfWork.Save();
                _bookUnitOfWork.Commit();

                response.BookID = book.BookId;
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _bookUnitOfWork.Rollback();
                s_logger.Error(ex, "Unable to add book.");
                response = new AddBookResponse();
            }

            return response;
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

        public BaseResponse UpdateBook(UpdateBookRequest request)
        {
            var response = new AddBookResponse();

            try
            {
                response = (AddBookResponse)request.ValidateRequest(response);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    return response;

                var book = _bookUnitOfWork.BookDataLayer.GetBook(request.BookID);

                if (book == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }

                if (!string.IsNullOrEmpty(request.ISBN))
                {
                    var existingISBN = _bookUnitOfWork.BookDataLayer.GetBookByISBN(request.ISBN);

                    if (existingISBN != null)
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.Messages.Add("Book with that ISBN already exists.");
                        return response;
                    }

                }

                if (!string.IsNullOrEmpty(request.eISBN))
                {
                    var existingeISBN = _bookUnitOfWork.BookDataLayer.GetBookByeISBN(request.eISBN);

                    if (existingeISBN != null)
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.Messages.Add("Book with that eISBN already exists.");
                        return response;
                    }

                }


                book.CoverImage = request.CoverImage == null ? null : Convert.ToBase64String(request.CoverImage);
                book.CreatedBy = int.Parse(_principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
                book.CreatedDate = DateTime.Now;
                book.Edition = request.Edition;
                book.EIsbn = request.eISBN;
                book.FictionTypeId = request.FictionTypeID;
                book.FormTypeId = request.FormTypeID;
                book.Isbn = request.ISBN;
                book.NumberInSeries = request.NumberInSeries;
                book.PublicationFormatId = request.PublicationFormatID;
                book.PublisherId = request.PublisherIDs;
                book.SeriesId = request.SeriesID;
                book.Subtitle = request.Subtitle;
                book.Title = request.Title;

                _bookUnitOfWork.BookDataLayer.DeleteBookAuthorRelationships(request.BookID);
                _bookUnitOfWork.BookDataLayer.DeleteBookGenreRelationships(request.BookID);

                _bookUnitOfWork.Begin();

                foreach (int authorId in request.Authors)
                {
                    _bookUnitOfWork.BookDataLayer.AddBookAuthor(new BookAuthor()
                    {
                        AuthorId = authorId,
                        BookId = book.BookId,
                    });
                }

                foreach (int genreId in request.Genres)
                {
                    _bookUnitOfWork.BookDataLayer.AddBookGenre(new BookGenre()
                    {
                        GenreId = genreId,
                        BookId = book.BookId,
                    });
                }

                _bookUnitOfWork.Save();
                _bookUnitOfWork.Commit();

                response.BookID = book.BookId;
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _bookUnitOfWork.Rollback();
                s_logger.Error(ex, "Unable to update book.");
                response = new AddBookResponse();
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
