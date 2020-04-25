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
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorUnitOfWork _authorUnitOfWork;
        private readonly ClaimsPrincipal _principal;

        protected static Logger s_logger = LogManager.GetCurrentClassLogger();

        public AuthorService(IAuthorUnitOfWork authorUnitOfWork, ClaimsPrincipal principal)
        {
            _authorUnitOfWork = authorUnitOfWork;
            _principal = principal;
        }

        public AddAuthorResponse AddAuthor(AddAuthorRequest request)
        {
            var response = new AddAuthorResponse();

            try
            {
                response = (AddAuthorResponse)request.ValidateRequest(response);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    return response;

                var author = new Author()
                {
                    FirstName = request.Firstname,
                    MiddleName = request.Middlename,
                    LastName = request.Lastname,
                    CountryId = request.CountryID,
                    Description = request.Description,
                    CreatedDate = DateTime.Now,
                    CreatedBy = int.Parse(_principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value),
                };

                _authorUnitOfWork.AuthorDataLayer.AddAuthor(author);
                _authorUnitOfWork.Save();

                response.AuthorID = author.AuthorId;
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to add author.");
                response = new AddAuthorResponse();
            }
            return response;
        }

        public BaseResponse DeleteAuthor(int id)
        {
            var response = new BaseResponse();
            try
            {
                var author = _authorUnitOfWork.AuthorDataLayer.GetAuthor(id);
                if (author == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }

                _authorUnitOfWork.AuthorDataLayer.DeleteAuthor(id);
                _authorUnitOfWork.Save();

                response.StatusCode = HttpStatusCode.OK;
                return response;
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to delete author.");
                response = new BaseResponse();
            }
            return response;
        }

        public GetAuthorResponse GetAuthor(int id)
        {
            var response = new GetAuthorResponse();
            try
            {
                var author = _authorUnitOfWork.AuthorDataLayer.GetAuthor(id);

                if (author == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }

                response.Author = DAO2DTO(author);
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to find author.");
                response = new GetAuthorResponse();
            }
            return response;
        }

        public GetAuthorsResponse GetAuthors()
        {
            var response = new GetAuthorsResponse();
            try
            {
                var authors = _authorUnitOfWork.AuthorDataLayer.GetAuthors();

                if (authors.Count == 0)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }

                response.Authors = authors.Select(a => DAO2ListDTO(a)).ToList();
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to find authors.");
                response = new GetAuthorsResponse();
            }
            return response;
        }

        public BaseResponse UpdateAuthor(UpdateAuthorRequest request)
        {
            var response = new BaseResponse();

            try
            {
                response = request.ValidateRequest(response);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    return response;

                var author = _authorUnitOfWork.AuthorDataLayer.GetAuthor(request.AuthorID);

                if (author == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }

                author.FirstName = request.FirstName;
                author.MiddleName = request.MiddleName;
                author.LastName = request.LastName;
                author.CountryId = request.CountryID;
                author.Description = request.Description;
                author.ModifiedBy = int.Parse(_principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
                author.ModifiedDate = DateTime.Now;

                _authorUnitOfWork.Save();

                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to update author.");
                response = new GetGenresResponse();
            }

            return response;
        }

        private AuthorDTO DAO2DTO(Author author)
        {
            return new AuthorDTO()
            {
                AuthorID = author.AuthorId,
                FirstName = author.FirstName,
                Description = author.Description,
                CountryID = author.CountryId,
                LastName = author.LastName,
                MiddleName = author.MiddleName
            };
        }

        private AuthorListItemDTO DAO2ListDTO(Author author)
        {
            return new AuthorListItemDTO()
            {
                AuthorID = author.AuthorId,
                Country = author.Country.Name,
                Name = ($"{author.FirstName} {author.MiddleName} {author.LastName}").Trim(),
            };
        }
    }
}
