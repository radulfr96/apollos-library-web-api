using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyLibrary.Common.DTOs;
using MyLibrary.Common.Requests;
using MyLibrary.Common.Responses;
using MyLibrary.Contracts.UnitOfWork;
using MyLibrary.Data.Model;
using MyLibrary.DataLayer.Contracts;
using MyLibrary.Services.Contracts;
using NLog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace MyLibrary.Services
{
    public class UserService : IUserService
    {
        private IUserUnitOfWork _userUnitOfWork;
        private IConfiguration _configuration;

        protected static Logger s_logger = LogManager.GetCurrentClassLogger();

        public UserService(IUserUnitOfWork userUnitOfWork, IConfiguration configuration)
        {
            _userUnitOfWork = userUnitOfWork;
            _configuration = configuration;
        }

        public GetUsersResponse GetUsers()
        {
            var response = new GetUsersResponse();
            try
            {
                var users = _userUnitOfWork.UserDataLayer.GetUsers();

                if (users.Count == 0)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Messages.Add("No users found");
                    return response;
                }

                foreach (User user in users)
                {
                    response.Users.Add(DAO2DTO(user));
                }

                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to retreive users.");
                response = new GetUsersResponse();
            }
            return response;
        }

        public LoginResponse Login(LoginRequest request)
        {
            s_logger.Info("Logging in user");

            var response = new LoginResponse();

            try
            {
                response = (LoginResponse)request.ValidateRequest(response);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    return response;

                var user = _userUnitOfWork.UserDataLayer.GetUserByUsername(request.Username);

                if (user == null)
                {
                    s_logger.Warn($"Unable to login as username [ {request.Username} ], username or password is incorrect.");
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }

                var hashedPassword = hashPassword(request.Password, user.Salter);

                if (user.Password != hashedPassword)
                {
                    s_logger.Warn($"Unable to login as username [ {request.Username} ], username or password is incorrect.");
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }

                if (user.SetPassword)
                {
                    response.StatusCode = HttpStatusCode.Accepted;
                    return response;
                }

                var handler = new JwtSecurityTokenHandler();


                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, user.Username));

                foreach (UserRole userRole in user.UserRole)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));
                }

                var key = Encoding.ASCII.GetBytes(_configuration.GetValue(typeof(string), "TokenKey").ToString());

                var identity = new ClaimsIdentity(claims);

                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = identity,
                    Expires = DateTime.Now.AddMonths(3),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                };

                var rawToken = handler.CreateJwtSecurityToken(tokenDescriptor);
                response.Token = handler.WriteToken(rawToken);
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to login user.");
                response = new LoginResponse();
            }

            return response;
        }

        private UserDTO DAO2DTO(User user)
        {
            return new UserDTO()
            {
                UserID = user.UserId,
                IsActive = user.IsActive ? "Active" : "Inactive",
                SetPassword = user.SetPassword,
                Username = user.Username
            };
        }

        private string hashPassword(string password, string salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }
    }
}
