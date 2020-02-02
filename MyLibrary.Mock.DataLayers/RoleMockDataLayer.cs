using MyLibrary.Data.Model;
using MyLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLibrary.Mock.DataLayers
{
    public class RoleMockDataLayer : IRoleDataLayer
    {
        private List<Role> Roles = new List<Role>();
        private List<UserRole> UserRoles = new List<UserRole>();

        public RoleMockDataLayer(List<Role> roles, List<UserRole> userRoles)
        {
            Roles = roles;
            UserRoles = userRoles;
        }

        public void AddRole(Role role)
        {
            Roles.Add(role);
        }

        public Role GetRole(int roleId)
        {
            return Roles.FirstOrDefault(r => r.RoleId == roleId);
        }

        public List<Role> GetRoles()
        {
            return Roles;
        }

        public List<UserRole> GetUserRoles(int userId)
        {
            return UserRoles.Where(ur => ur.UserId == userId).ToList();
        }

        public void Save() {}
    }
}
