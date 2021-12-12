using Microsoft.AspNetCore.Identity;
using MyLibrary.Application.Common.Functions;
using MyLibrary.IDP.Model;
using MyLibrary.IDP.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.IDP.Services
{
    public class UserService : IUserService
    {
        private readonly IUserUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(IUserUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public Guid GetUserId()
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _unitOfWork.UserDataLayer.GetUserByUsername(username);
        }

        public async Task<bool> ValidateCredentials(string username, string password)
        {
            var result = false;

            var user = await GetUserByUsername(username);

            if (user == null)
                return result;

            if (!user.IsActive)
                return result;

            result = _passwordHasher.VerifyHashedPassword(user, user.Password, password) == PasswordVerificationResult.Success;

            return result;
        }

        public async Task<List<UserClaim>> GetUserClaimsBySubject(string subject)
        {
            return await _unitOfWork.UserDataLayer.GetUserClaimsBySubject(subject);
        }

        public async Task<bool> IsUserActive(string subject)
        {
            var user = await GetUserBySubject(subject);

            if (user == null)
            {
                return false;
            }

            return user.IsActive;
        }

        public async Task<User> GetUserBySubject(string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            return await _unitOfWork.UserDataLayer.GetUserBySubject(subject);
        }

        public async Task AddUser(User user, string password)
        {
            user.Password = _passwordHasher.HashPassword(user, password);
            await _unitOfWork.UserDataLayer.AddUser(user);
            await _unitOfWork.Save();
        }

        public async Task<bool> ActivateUser(string securityCode)
        {
            var user = await _unitOfWork.UserDataLayer.GetUserBySecurityCode(securityCode);

            if (user == null)
            {
                return false;
            }

            user.IsActive = true;
            user.SecurityCode = null;

            await _unitOfWork.Save();

            return true;
        }

        public async Task<string> InitiatePasswordResetRequest(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            var user = await _unitOfWork.UserDataLayer.GetUserByUsername(email);

            if (user == null)
            {
                throw new Exception($"User with email address {email} can't be found.");
            }

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                var securityCodeData = new byte[128];
                randomNumberGenerator.GetBytes(securityCodeData);
                user.SecurityCode = Convert.ToBase64String(securityCodeData);
            }

            user.SecurityCodeExpirationDate = DateTime.Now.AddHours(1);

            await _unitOfWork.Save();
            return user.SecurityCode;
        }

        public async Task<bool> SetPassword(string securityCode, string password)
        {
            if (string.IsNullOrWhiteSpace(securityCode))
            {
                throw new ArgumentNullException(nameof(securityCode));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            var user = await _unitOfWork.UserDataLayer.GetUserBySecurityCode(securityCode);

            if (user == null)
            {
                return false;
            }

            if (!user.IsActive)
            {
                user.IsActive = true;
            }

            user.SecurityCode = null;
            // hash & salt the password
            user.Password = _passwordHasher.HashPassword(user, password);

            await _unitOfWork.Save();
            return true;

        }
    }
}
