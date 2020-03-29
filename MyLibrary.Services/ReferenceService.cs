using MyLibrary.Common.DTOs;
using MyLibrary.Common.Responses;
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
    public class ReferenceService : IReferenceService
    {
        private readonly IReferenceUnitOfWork _referenceUnitOfWork;
        private readonly ClaimsPrincipal _principal;
        protected static Logger s_logger = LogManager.GetCurrentClassLogger();

        public ReferenceService(IReferenceUnitOfWork referenceUnitOfWork, ClaimsPrincipal principal)
        {
            _referenceUnitOfWork = referenceUnitOfWork;
            _principal = principal;
        }

        public GetCountriesResponse GetCountries()
        {
            var response = new GetCountriesResponse();

            try
            {
                var countries = _referenceUnitOfWork.ReferenceDataLayer.GetCountries();

                if (countries.Count == 0)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.Countries = countries.Select(c => new CountryDTO()
                    {
                        CountryID = c.CountryId,
                        Name = c.Name,
                    })
                    .ToList();
                }
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to get countries.");
                response = new GetCountriesResponse();
            }

            return response;
        }
    }
}
