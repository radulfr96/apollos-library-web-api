using Microsoft.Extensions.Logging;
using MyLibrary.Common.Responses;
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

        public GetAuthorResponse GetBook(int id)
        {
            var response = new GetGenreResponse();
            try
            {
                var genre = _bookUnitOfWork.GenreDataLayer.GetGenre(id);

                if (genre == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }

                response.Genre = DAO2DTO(genre);
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to find book.");
                response = new GetGenreResponse();
            }
            return response;
        }
    }
}
