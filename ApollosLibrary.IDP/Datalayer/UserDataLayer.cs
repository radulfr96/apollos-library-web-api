using Microsoft.EntityFrameworkCore;
using ApollosLibrary.IDP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApollosLibrary.IDP.DataLayer
{
    public class UserDataLayer : IUserDataLayer
    {
        private ApollosLibraryContext _context;

        public UserDataLayer(ApollosLibraryContext context)
        {
            _context = context;
        }

        public async Task AddUser(Model.User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<Model.User> GetUser(Guid id)
        {
            return await _context.Users
                                 .Include(u => u.UserClaims)
                                 .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<List<Model.User>> GetUsers()
        {
            return await (
                from u in _context.Users
                select new Model.User()
                {
                    CreatedBy = u.CreatedBy,
                    CreatedDate = u.CreatedDate,
                    IsActive = u.IsActive,
                    ModifiedBy = u.ModifiedBy,
                    ModifiedDate = u.ModifiedDate,
                    Password = u.Password,
                    UserId = u.UserId,
                    Username = u.Username,
                }).ToListAsync();
        }

        public async Task<Model.User> GetUserBySubject(string subject)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Subject == subject);
        }

        public async Task<Model.User> GetUserByUsername(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user != null)
            {
                user.UserClaims = await _context.UserClaims.Where(u => u.UserId == user.UserId).ToListAsync();
                user.UserClaims.Add(new UserClaim() { User = user, Value = username, Type = "username" });
                user.UserClaims.Add(new UserClaim() { User = user, Value = user.UserId.ToString(), Type = "userid" });
            }

            return user;
        }

        public async Task<Model.User> GetUserByEmail(string email)
        {
            var emailClaim = await _context.UserClaims.Where(u => u.Value == email && u.Type == "emailaddress").FirstOrDefaultAsync();

            if (emailClaim == null)
                return null;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == emailClaim.UserId);

            if (user != null)
            {
                user.UserClaims = await _context.UserClaims.Where(u => u.UserId == user.UserId).ToListAsync();
                user.UserClaims.Add(new UserClaim() { User = user, Value = user.Username, Type = "username" });
                user.UserClaims.Add(new UserClaim() { User = user, Value = user.UserId.ToString(), Type = "userid" });
            }

            return user;
        }

        public async Task<Model.User> GetUserByEmailUserOnly(string email)
        {
            var emailClaim = await _context.UserClaims.Where(u => u.Value == email && u.Type == "emailaddress").FirstOrDefaultAsync();

            if (emailClaim == null)
                return null;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == emailClaim.UserId);

            return user;
        }

        public async Task<List<UserClaim>> GetUserClaimsBySubject(string subject)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Subject == subject);

            if (user == null)
                return new List<UserClaim>();

            var claims = await _context.UserClaims.Where(u => u.User.Subject == subject).ToListAsync();
            claims.Add(new UserClaim() { User = user, Value = user.Username, Type = "username" });
            claims.Add(new UserClaim() { User = user, Value = user.UserId.ToString(), Type = "userid" });

            return claims;
        }

        public async Task<Model.User> GetUserBySecurityCode(string securityCode)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.SecurityCode == securityCode && u.SecurityCodeExpirationDate >= DateTime.Now);
        }
    }
}