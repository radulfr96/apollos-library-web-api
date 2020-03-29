using MyLibrary.Common.DTOs;
using MyLibrary.Common.Requests;
using MyLibrary.Common.Responses;
using MyLibrary.Data.Model;
using MyLibrary.Services.Contracts;
using MyLibrary.UnitOfWork.Contracts;
using NLog;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;

namespace MyLibrary.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IPublisherUnitOfWork _publisherUnitOfWork;
        private readonly ClaimsPrincipal _principal;

        protected static Logger s_logger = LogManager.GetCurrentClassLogger();

        public PublisherService(IPublisherUnitOfWork publisherUnitOfWork, ClaimsPrincipal principal)
        {
            _publisherUnitOfWork = publisherUnitOfWork;
            _principal = principal;
        }

        public BaseResponse AddPublisher(AddPublisherRequest request)
        {
            var response = new BaseResponse();

            try
            {
                response = request.ValidateRequest(response);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    return response;

                var publisher = new Publisher()
                {
                    CreatedBy = int.Parse(_principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value),
                    CreateDate = DateTime.Now,
                    City = request.City,
                    CountryId = request.CountryID,
                    IsDeleted = false,
                    Postcode = request.Postcode,
                    State = request.State,
                    StreetAddress = request.StreetAddress,
                    Website = request.Website,
                    Name = request.Name,
                };

                _publisherUnitOfWork.PublisherDataLayer.AddPublisher(publisher);
                _publisherUnitOfWork.Save();

                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to add publisher.");
                response = new BaseResponse();
            }
            return response;
        }

        public BaseResponse DeletePublisher(int id)
        {
            var response = new BaseResponse();

            try
            {
                var publisher = _publisherUnitOfWork.PublisherDataLayer.GetPublisher(id);

                if (publisher == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }

                publisher.IsDeleted = true;
                publisher.Name = "Deleted";
                publisher.City = "Deleted";
                publisher.Postcode = "0000";
                publisher.State = "Deleted";
                publisher.StreetAddress = "Deleted";
                publisher.Website = "";
                publisher.CountryId = "AU";

                _publisherUnitOfWork.Save();

                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to delete publisher.");
                response = new BaseResponse();
            }
            return response;
        }

        public GetPublisherResponse GetPublisher(int id)
        {
            var response = new GetPublisherResponse();

            try
            {
                var publisher = _publisherUnitOfWork.PublisherDataLayer.GetPublisher(id);

                if (publisher == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                }
                else
                {
                    response.Publisher = DAO2DTO(publisher);
                    response.StatusCode = HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to retreive publisher.");
                response = new GetPublisherResponse();
            }

            return response;
        }

        public GetPublishersResponse GetPublishers()
        {
            var response = new GetPublishersResponse();

            try
            {
                var publishers = _publisherUnitOfWork.PublisherDataLayer.GetPublishers();

                if (publishers.Count == 0)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                }
                else
                {
                    response.Publishers = publishers.Select(p => DAO2DTO(p)).ToList();
                    response.StatusCode = HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to retreive publishers.");
                response = new GetPublishersResponse();
            }

            return response;
        }

        public BaseResponse UpdatePublisher(UpdatePublisherRequest request)
        {
            var response = new BaseResponse();

            try
            {
                response = request.ValidateRequest(response);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    return response;

                var publisher = _publisherUnitOfWork.PublisherDataLayer.GetPublisher(request.PublisherID);

                if (publisher == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }

                publisher.City = request.City;
                publisher.CountryId = request.CountryID;
                publisher.IsDeleted = true;
                publisher.ModifiedBy = int.Parse(_principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
                publisher.ModifiedDate = DateTime.Now;
                publisher.Name = request.Name;
                publisher.Postcode = request.Postcode;
                publisher.State = request.State;
                publisher.StreetAddress = request.StreetAddress;
                publisher.Website = request.Website;
                _publisherUnitOfWork.Save();

                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to update publisher.");
                response = new BaseResponse();
            }
            return response;
        }

        private PublisherDTO DAO2DTO(Publisher publisher)
        {
            return new PublisherDTO()
            {
                City = publisher.City,
                Country = new CountryDTO()
                {
                    CountryID = publisher.Country.CountryId,
                    Name = publisher.Name,
                },
                Name = publisher.Name,
                Postcode = publisher.Postcode,
                PublisherID = publisher.PublisherId,
                State = publisher.State,
                StreetAddress = publisher.StreetAddress,
                Website = publisher.Website,
            };
        }
    }
}
