using MyLibrary.Data.Model;
using System;
using System.Collections.Generic;

namespace MyLibrary.DataLayer.Contracts
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
        public List<User> GetUsers();
        
        /// <summary>
        /// Used to get a user by their id
        /// </summary>
        /// <param name="id">The id of the user to be retreived</param>
        /// <returns>The user with the id</returns>
        public User GetUser(int id);

        /// <summary>
        /// Used to get a user by their username
        /// </summary>
        /// <param name="username">The users username</param>
        /// <returns>The user with the username</returns>
        public User GetUserByUsername(string username);

        /// <summary>
        /// Used to add a new user
        /// </summary>
        /// <param name="user">The user to be added</param>
        public void AddUser(User user);

        /// <summary>
        /// Used to save any changes made to data
        /// </summary>
        public void Save();
    }
}
