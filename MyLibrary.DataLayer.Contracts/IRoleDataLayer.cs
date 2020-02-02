using MyLibrary.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.DataLayer.Contracts
{
    /// <summary>
    /// Used to handle data storage of user roles
    /// </summary>
    public interface IRoleDataLayer
    {
        /// <summary>
        /// Used to add a new role
        /// </summary>
        /// <param name="role">The role to be added</param>
        public void AddRole(Role role);

        /// <summary>
        /// Used to get a role by it's id
        /// </summary>
        /// <param name="roleId">The id of the role to be found</param>
        /// <returns>The role that is found</returns>
        public Role GetRole(int roleId);

        /// <summary>
        /// Used to get all of the roles
        /// </summary>
        /// <returns>The list of user roles</returns>
        public List<Role> GetRoles();

        /// <summary>
        /// Used to get a users roles by their ids
        /// </summary>
        /// <param name="userId">The id of the user whose roles are to be found</param>
        /// <returns>The users roles found</returns>
        public List<UserRole> GetUserRoles(int userId);

        /// <summary>
        /// Used to save changes to the database
        /// </summary>
        public void Save();
    }
}
