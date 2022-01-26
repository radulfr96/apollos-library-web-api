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
using ApollosLibrary.Persistence.Model;
using ApollosLibrary.UnitOfWork;

namespace ApollosLibrary.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ReferenceController : BaseApiController
    {
        private readonly ApollosLibraryContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IReferenceDataService _referenceService;

        public ReferenceController(ApollosLibraryContext dbContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _referenceService = new ReferenceDataService(new ReferenceUnitOfWork(_dbContext));
        }

        [HttpGet("countries")]
        public async Task<List<CountryDTO>> GetCountries()
        {
            return await _referenceService.GetCountries();
        }
    }
}