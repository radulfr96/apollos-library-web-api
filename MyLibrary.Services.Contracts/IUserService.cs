using MyLibrary.Common.Requests;
using MyLibrary.Common.Responses;
using System;

namespace MyLibrary.Services.Contracts
{
    /// <summary>
    /// Used to performa business logic for users
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Used to retreive all users
        /// </summary>
        /// <returns>Reponse with the list of users</returns>
        public GetUsersResponse GetUsers();

        /// <summary>
        /// Used to login a user
        /// </summary>
        /// <param name="request">The users login information</param>
        /// <returns>The response with the users token</returns>
        public LoginResponse Login(LoginRequest request);
    }
}
