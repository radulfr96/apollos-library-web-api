using ApollosLibrary.Application.Common.DTOs;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Infrastructure.Services
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

        public async Task<BookReferenceDataDTO> GetBookReferenceData()
        {
            return new BookReferenceDataDTO()
            {
                FictionTypes = await _referenceUnitOfWork.ReferenceDataLayer.GetFictionTypes(),
                FormTypes = await _referenceUnitOfWork.ReferenceDataLayer.GetFormTypes(),
                PublicationFormats = await _referenceUnitOfWork.ReferenceDataLayer.GetPublicationFormats(),
            };
        }
    }
}
