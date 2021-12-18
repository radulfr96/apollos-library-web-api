using MyLibrary.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Infrastructure.Services
{
    public class UserService : IUserService
    {
        public Guid GetUserId()
        {
            var principal = Thread.CurrentPrincipal as ClaimsPrincipal;

            return Guid.Parse(principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
        }
    }
}
