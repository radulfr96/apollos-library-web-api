using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyLibrary.Common.DTOs;
using MyLibrary.Common.Requests;
using MyLibrary.Common.Responses;
using MyLibrary.Contracts.UnitOfWork;
using MyLibrary.Data.Enums;
using MyLibrary.Data.Model;
using MyLibrary.DataLayer.Contracts;
using MyLibrary.Services.Contracts;
using NLog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MyLibrary.Services
{
    public class UserService : IUserService
    {
        private readonly IUserUnitOfWork _userUnitOfWork;
        private readonly IConfiguration _configuration;

        protected static Logger s_logger = LogManager.GetCurrentClassLogger();

        public UserService(IUserUnitOfWork userUnitOfWork, IConfiguration configuration)
        {
            _userUnitOfWork = userUnitOfWork;
            _configuration = configuration;
        }

        public RegisterUserResponse Register(RegisterUserRequest request)
        {
            var response = new RegisterUserResponse();

            try
            {
                response = (RegisterUserResponse)request.ValidateRequest(response);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    return response;

                var user = _userUnitOfWork.UserDataLayer.GetUserByUsername(request.Username);

                if (user != null)
                {
                    s_logger.Warn($"Unable to register as username [ {request.Username} ], was taken.");
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Messages.Add("Registration unsuccessful user already exists");
                    return response;
                }

                byte[] salt = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }
                var saltString = Convert.ToBase64String(salt);

                var hashedPass = HashPassword(request.Password, saltString);

                var newUser = new User()
                {
                    CreatedBy = request.Username,
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Password = hashedPass,
                    Salter = saltString,
                    Username = request.Username
                };

                newUser.UserRole.Add(new UserRole()
                {
                    UserId = newUser.UserId,
                    RoleId = (int)RoleEnum.StandardUser,
                    Role = _userUnitOfWork.RoleDataLayer.GetRole((int)RoleEnum.StandardUser)
                });

                _userUnitOfWork.UserDataLayer.AddUser(newUser);
                _userUnitOfWork.Save();

                response.Token = GetToken(newUser);
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to register user.");
                response = new RegisterUserResponse();
            }

            return response;
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

        public UpdateUsernameResponse UpdateUsername(UpdateUsernameRequest request, int userId)
        {
            var response = new UpdateUsernameResponse();

            try
            {
                response = (UpdateUsernameResponse)request.ValidateRequest(response);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    return response;

                var existingUsername = _userUnitOfWork.UserDataLayer.GetUserByUsername(request.NewUsername);

                if (existingUsername != null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Messages.Add("Username already exists");
                    return response;
                }

                var user = _userUnitOfWork.UserDataLayer.GetUser(userId);

                if (user == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Messages.Add($"Unable to find user with id [{userId}]");
                    return response;
                }

                if (user.Password != HashPassword(request.Password, user.Salter))
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Messages.Add("Password is not correct");
                    return response;
                }

                user.Username = request.NewUsername;
                user.ModifiedDate = DateTime.Now;
                user.ModifiedBy = request.NewUsername;

                _userUnitOfWork.Save();

                response.Token = GetToken(user);
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to update username.");
                response = new UpdateUsernameResponse();
            }

            return response;
        }

        public BaseResponse UpdatePassword(UpdatePasswordRequest request, int userId)
        {
            var response = new BaseResponse();

            try
            {
                response = request.ValidateRequest(response);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    return response;

                var user = _userUnitOfWork.UserDataLayer.GetUser(userId);

                if (user == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Messages.Add($"Unable to find user with id [{userId}]");
                    return response;
                }

                if (user.Password != HashPassword(request.Password, user.Salter))
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Messages.Add("Password is not correct");
                    return response;
                }

                user.Password = HashPassword(request.Password, user.Salter);
                user.ModifiedDate = DateTime.Now;
                user.ModifiedBy = user.Username;

                _userUnitOfWork.Save();

                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to update username.");
                response = new BaseResponse();
            }

            return response;
        }

        public GetUserResponse GetUserById(int id)
        {
            var response = new GetUserResponse();

            try
            {
                var user = _userUnitOfWork.UserDataLayer.GetUser(id);

                if (user == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Messages.Add($"Unable to finc user with id [{id}]");
                    return response;
                }

                response.User = DAO2DTO(user);
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to find user.");
                response = new GetUserResponse();
            }
            return response;
        }

        public UsernameCheckResponse UsernameCheck(string username)
        {
            var response = new UsernameCheckResponse();

            try
            {
                var user = _userUnitOfWork.UserDataLayer.GetUserByUsername(username);

                if (user == null)
                {
                    response.Result = false;
                    response.StatusCode = HttpStatusCode.OK;
                    return response;
                }

                response.Result = true;
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to find user.");
                response = new UsernameCheckResponse();
            }
            return response;
        }

        public LoginResponse Login(LoginRequest request)
        {
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

                var hashedPassword = HashPassword(request.Password, user.Salter);

                if (user.Password != hashedPassword)
                {
                    s_logger.Warn($"Unable to login as username [ {request.Username} ], username or password is incorrect.");
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }

                response.Token = GetToken(user);
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to login user.");
                response = new LoginResponse();
            }

            return response;
        }

        private string GetToken(User user)
        {
            var handler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Sid, user.UserId.ToString()),
                    new Claim("JoinDate", user.CreatedDate.ToString())
                };

            var key = Encoding.ASCII.GetBytes(_configuration.GetValue(typeof(string), "TokenKey").ToString());

            var identity = new ClaimsIdentity(claims);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = identity,
                Expires = DateTime.Now.AddMonths(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var rawToken = handler.CreateJwtSecurityToken(tokenDescriptor);
            return handler.WriteToken(rawToken);
        }

        private UserDTO DAO2DTO(User user)
        {
            var roles = new List<RoleDTO>();

            foreach (var userRole in user.UserRole)
            {
                roles.Add(new RoleDTO()
                {
                    RoleId = userRole.RoleId,
                    Name = userRole.Role.Name
                });
            }

            return new UserDTO()
            {
                UserID = user.UserId,
                IsActive = user.IsActive ? "Active" : "Inactive",
                Username = user.Username,
                Roles = roles
            };
        }

        private string HashPassword(string password, string salt)
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
