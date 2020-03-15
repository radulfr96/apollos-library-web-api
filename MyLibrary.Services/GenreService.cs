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

                response.Genre = DAO2DTO(genre);
                response.StatusCode = HttpStatusCode.OK;
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
            var response = new GetGenresResponse();
            try
            {
                var genres = _genreUnitOfWork.GenreDataLayer.GetGenres();

                if (genres.Count == 0)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }

                foreach (var genre in genres)
                {
                    response.Genres.Add(DAO2DTO(genre));
                }

                response.StatusCode = HttpStatusCode.OK; 
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to find genres.");
                response = new GetGenresResponse();
            }
            return response;
        }

        public BaseResponse UpdateGenre(UpdateGenreRequest request)
        {
            var response = new BaseResponse();

            try
            {
                response = request.ValidateRequest(response);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    return response;

                var genre = _genreUnitOfWork.GenreDataLayer.GetGenre(request.GenreID);

                if (genre == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }

                genre.Name = request.Name;
                genre.ModifiedBy = _principal.Identity.Name;
                genre.ModifiedDate = DateTime.Now;

                _genreUnitOfWork.Save();

                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to update genre.");
                response = new GetGenresResponse();
            }

            return response;
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
