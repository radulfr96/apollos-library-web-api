using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ApollosLibrary.Application.Common.DTOs;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Infrastructure.Services;
using ApollosLibrary.UnitOfWork;
using ApollosLibrary.Domain;

namespace ApollosLibrary.WebApi.Controllers
{
    /// <summary>
    /// Used to get reference data used by the system
    /// </summary>
    [Route("api/[controller]")]
    public class ReferenceController : BaseApiController
    {
        private readonly ApollosLibraryContext _dbContext;
        private readonly IReferenceDataService _referenceService;

        public ReferenceController(ApollosLibraryContext dbContext, IConfiguration configuration) : base(configuration)
        {
            _dbContext = dbContext;
            _referenceService = new ReferenceDataService(new ReferenceUnitOfWork(_dbContext));
        }

        /// <summary>
        /// Used to get countries used by the system
        /// </summary>
        /// <returns>The list of countries</returns>
        [HttpGet("countries")]
        public async Task<List<CountryDTO>> GetCountries()
        {
            return await _referenceService.GetCountries();
        }

        /// <summary>
        /// Used to get the business types used in tne system
        /// </summary>
        /// <returns></returns>
        [HttpGet("businesstypes")]
        public async Task<List<BusinessTypeDTO>> GetBusinessTypes()
        {
            return await _referenceService.GetBusinessTypes();
        }

        /// <summary>
        /// Used to get the various types used by book entries
        /// </summary>
        /// <returns>The data used by book management</returns>
        [HttpGet("bookReferenceData")]
        public async Task<BookReferenceDataDTO> GetBookReferenceData()
        {
            return await _referenceService.GetBookReferenceData();
        }
    }
}