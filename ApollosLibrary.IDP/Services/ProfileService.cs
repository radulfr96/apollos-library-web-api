using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace ApollosLibrary.IDP.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserService _userService;

        public ProfileService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.GetSubjectId();

            var claims = await _userService.GetUserClaimsBySubject(subjectId);

            context.IssuedClaims.AddRange(claims.Select(c => new Claim(c.Type, c.Value)));
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject.GetSubjectId();
            context.IsActive = await _userService.IsUserActive(subject);
        }
    }
}
