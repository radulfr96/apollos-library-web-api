using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ApollosLibrary.WebApi.Filters;
using NLog;
using Microsoft.AspNetCore.Authorization;

namespace ApollosLibrary.WebApi.Controllers
{
    /// <summary>
    /// This is used as the base for all of the api controllers as all them will need
    /// the methods and attributes contained within this object
    /// </summary>
    [Authorize]
    [ApiController]
    [ServiceFilter(typeof(ApiExceptionFilterAttribute))]
    public class BaseApiController : ControllerBase
    {
        protected readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IConfiguration _config;

        public BaseApiController(IConfiguration config)
        {
            _config = config;
        }

        protected bool UserIsAdmin()
        {
            bool result = false;
            try
            {
                var claims = HttpContext.User.Claims;
                var roles = claims.Where(u => u.Type == ClaimTypes.Role).ToList();
                var adminRole = _config.GetSection("AdminRoleName").Value;

                result = roles.Select(r => r.Value).Contains(adminRole);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unable to determine a users role");
            }

            return result;
        }
    }
}