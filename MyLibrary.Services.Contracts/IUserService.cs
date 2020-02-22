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

        /// <summary>
        /// Used to register user 
        /// </summary>
        /// <param name="request">The users registration information</param>
        /// <returns>The response with the users token</returns>
        public RegisterUserResponse Register(RegisterUserRequest request);

        /// <summary>
        /// Used to get the user by the id
        /// </summary>
        /// <param name="userId">The id of the user to be found</param>
        /// <returns>The response with the user found</returns>
        public GetUserResponse GetUserById(int userId);

        /// <summary>
        /// Used to check if the username exists
        /// </summary>
        /// <param name="username">The username to be checked</param>
        /// <returns>The response with the result</returns>
        public UsernameCheckResponse UsernameCheck(string username);

        /// <summary>
        /// Used to update the users username
        /// </summary>
        /// <param name="request">The update information</param>
        /// <param name="userId">The id of the user to be updated</param>
        /// <returns>The response with the result information</returns>
        public UpdateUsernameResponse UpdateUsername(UpdateUsernameRequest request, int userId);
    }
}
