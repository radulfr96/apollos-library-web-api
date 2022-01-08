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
        /// Used to get a user by their username
        /// </summary>
        /// <param name="username">The users username</param>
        /// <returns>The user with the username</returns>
        Task<User> GetUserByUsername(string username);

        /// <summary>
        /// Used to get a user by their email
        /// </summary>
        /// <param name="email">The users email</param>
        /// <returns>The user with the email</returns>
        Task<User> GetUserByEmail(string email);

        /// <summary>
        /// Used to get a user by their email without their claims
        /// </summary>
        /// <param name="email">the users email</param>
        /// <returns>The users record</returns>
        Task<User> GetUserByEmailUserOnly(string email);

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
