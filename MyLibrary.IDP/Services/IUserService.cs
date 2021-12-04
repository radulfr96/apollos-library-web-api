using MyLibrary.IDP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.IDP.Services
{
    public interface IUserService
    {
        Task<User> GetUserByUsername(string username);

        Task<bool> ValidateCredentials(string username, string password);

        Task<List<UserClaim>> GetUserClaimsBySubject(string subject);

        Guid GetUserId();

        Task<bool> IsUserActive(string subject);

        Task<bool> ActivateUser(string securityCode);

        Task AddUser(User user, string password);

        Task<string> InitiatePasswordResetRequest(string email);

        Task<bool> SetPassword(string securityCode, string password);

    }
}
