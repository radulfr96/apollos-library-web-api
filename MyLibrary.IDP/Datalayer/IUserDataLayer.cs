using MyLibrary.IDP.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyLibrary.IDP.DataLayer
{
    /// <summary>
    /// Used to handle data storage of users
    /// </summary>
    public interface IUserDataLayer
    {
        /// <summary>
        /// Retreives all users
        /// </summary>
        /// <returns>The list of users</returns>
        Task<List<User>> GetUsers();

        /// <summary>
        /// Used to get a user by their id
        /// </summary>
        /// <param name="id">The id of the user to be retreived</param>
        /// <returns>The user with the id</returns>
        Task<User> GetUser(Guid id);

        /// <summary>
        /// Used to get a user by their username
        /// </summary>
        /// <param name="username">The users username</param>
        /// <returns>The user with the username</returns>
        Task<User> GetUserByUsername(string username);

        /// <summary>
        /// Used to add a new user
        /// </summary>
        /// <param name="user">The user to be added</param>
        Task AddUser(User user);

        Task<User> GetUserBySubject(string subject);

        Task<List<UserClaim>> GetUserClaimsBySubject(string subject);

        Task<User> GetUserBySecurityCode(string securityCode);
    }
}
