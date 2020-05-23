using Microsoft.Extensions.Logging;
using MyLibrary.Common.DTOs;
using MyLibrary.Common.Responses;
using MyLibrary.Data.Model;
using MyLibrary.Services.Contracts;
using MyLibrary.UnitOfWork.Contracts;
using NLog;
using System;
using System.Collections.Generic;
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
                var genre = _bookUnitOfWork.BookDataLayer.GetBook(id);

                if (genre == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }

                response.Book = DAO2DTO(genre);
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to find book.");
                response = new GetBookResponse();
            }
            return response;
        }

        public BookDTO DAO2DTO(Book book)
        {
            return new BookDTO()
            {
                BookID = book.BookId,
                CoverImage = Encoding.ASCII.GetBytes(book.CoverImage),
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
            };
        }
    }
}
