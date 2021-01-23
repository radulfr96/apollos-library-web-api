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
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace MyLibrary.Services
{
    public class GenreService
    {
        private readonly IGenreUnitOfWork _genreUnitOfWork;
        private readonly ClaimsPrincipal _principal;

        protected static Logger s_logger = LogManager.GetCurrentClassLogger();

        public GenreService(IGenreUnitOfWork genreUnitOfWork, ClaimsPrincipal principal)
        {
            _genreUnitOfWork = genreUnitOfWork;
            _principal = principal;
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
