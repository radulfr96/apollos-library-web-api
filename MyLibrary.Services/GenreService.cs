using Microsoft.Extensions.Logging;
using MyLibrary.Common.DTOs;
using MyLibrary.Common.Requests;
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
    public class GenreService : IGenreService
    {
        private readonly IGenreUnitOfWork _genreUnitOfWork;
        private readonly ClaimsPrincipal _principal;

        protected static Logger s_logger = LogManager.GetCurrentClassLogger();

        public GenreService(IGenreUnitOfWork genreUnitOfWork, ClaimsPrincipal principal)
        {
            _genreUnitOfWork = genreUnitOfWork;
            _principal = principal;
        }

        public AddGenreResponse AddGenre(AddGenreRequest request)
        {
            var response = new AddGenreResponse();

            try
            {
                response = (AddGenreResponse)request.ValidateRequest(response);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    return response;

                var genre = new Genre()
                {
                    Name = request.Name,
                    CreateDate = DateTime.Now,
                    CreatedBy = _principal.Identity.Name,
                };

                _genreUnitOfWork.GenreDataLayer.AddGenre(genre);
                _genreUnitOfWork.Save();

                response.GenreID = genre.GenreId;
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to add genre.");
                response = new AddGenreResponse();
            }
            return response;
        }

        public BaseResponse DeleteGenre(int id)
        {
            var response = new BaseResponse();
            try
            {
                var genre = _genreUnitOfWork.GenreDataLayer.GetGenre(id);
                if (genre == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }

                _genreUnitOfWork.GenreDataLayer.DeleteGenre(id);
                _genreUnitOfWork.Save();

                response.StatusCode = HttpStatusCode.OK;
                return response;
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to delete genre.");
                response = new BaseResponse();
            }
            return response;
        }

        public GetGenreResponse GetGenre(int id)
        {
            var response = new GetGenreResponse();
            try
            {
                var genre = _genreUnitOfWork.GenreDataLayer.GetGenre(id);

                if (genre == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }

                response.StatusCode = HttpStatusCode.OK;
                response.Genre = DAO2DTO(genre);
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to find genre.");
                response = new GetGenreResponse();
            }
            return response;
        }

        public GetGenresResponse GetGenres()
        {
            throw new NotImplementedException();
        }

        public BaseResponse UpdateGenre(UpdateGenreRequest request)
        {
            throw new NotImplementedException();
        }

        private GenreDTO DAO2DTO(Genre genre)
        {
            return new GenreDTO()
            {
                GenreId = genre.GenreId,
                Name = genre.Name,
            };
        }
    }
}
