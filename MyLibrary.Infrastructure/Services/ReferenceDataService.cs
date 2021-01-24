using MyLibrary.Application.Common.DTOs;
using MyLibrary.Application.Interfaces;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Infrastructure.Services
{
    public class ReferenceDataService : IReferenceDataService
    {
        private readonly IReferenceUnitOfWork _referenceUnitOfWork;

        public ReferenceDataService(IReferenceUnitOfWork referenceUnitOfWork)
        {
            _referenceUnitOfWork = referenceUnitOfWork;
        }

        public string GetReferenceData()
        {
            return "{}";
        }

        public async Task<List<CountryDTO>> GetCountries()
        {
            var response = new List<CountryDTO>();

            var countries = await _referenceUnitOfWork.ReferenceDataLayer.GetCountries();

            if (countries.Count == 0)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }

            response = countries.Select(c => new CountryDTO()
            {
                CountryID = c.CountryId,
                Name = c.Name,
            })
            .ToList();

            return response;
        }
    }
}
