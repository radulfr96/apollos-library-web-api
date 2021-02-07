using MyLibrary.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;

namespace MyLibrary.Infrastructure.Services
{
    public class UserService : IUserService
    {
        public int GetUserId()
        {
            var principal = (ClaimsPrincipal)Thread.CurrentPrincipal;

            return int.Parse(principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
        }

        public string GetUsername()
        {
            var principal = (ClaimsPrincipal)Thread.CurrentPrincipal;

            return principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
        }
    }
}
