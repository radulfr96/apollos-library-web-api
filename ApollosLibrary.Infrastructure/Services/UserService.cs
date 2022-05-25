using Microsoft.AspNetCore.Http;
using ApollosLibrary.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly HttpContext _httpContext;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        public Guid GetUserId()
        {
            return Guid.Parse(_httpContext.User.Claims.FirstOrDefault(c => c.Type == "userid").Value);
        }
    }
}
