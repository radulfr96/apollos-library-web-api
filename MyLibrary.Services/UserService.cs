using MyLibrary.Common.DTOs;
using MyLibrary.Common.Responses;
using MyLibrary.Contracts.UnitOfWork;
using MyLibrary.Data.Model;
using MyLibrary.DataLayer.Contracts;
using NLog;
using System;
using System.Collections.Generic;
using System.Net;

namespace MyLibrary.Services
{
    public class UserService
    {
        private IUserUnitOfWork _userUnitOfWork;

        protected static Logger s_logger = LogManager.GetCurrentClassLogger();

        public UserService(IUserUnitOfWork userUnitOfWork)
        {
            _userUnitOfWork = userUnitOfWork;
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

        private UserDTO DAO2DTO(User user)
        {
            return new UserDTO()
            {
                UserID = user.UserId,
                IsActive = user.IsActive ? "Active": "Inactive",
                SetPassword = user.SetPassword,
                Username = user.Username
            };
        }
    }
}
