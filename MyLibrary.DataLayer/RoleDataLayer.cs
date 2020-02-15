using MyLibrary.Data.Model;
using MyLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLibrary.DataLayer
{
    public class RoleDataLayer : IRoleDataLayer
    {
        private MyLibraryContext _context;

        public RoleDataLayer(MyLibraryContext context)
        {
            _context = context;
        }

        public void AddRole(Role role)
        {
            _context.Add(role);
        }

        public Role GetRole(int roleId)
        {
            return _context.Role.FirstOrDefault(r => r.RoleId == roleId);
        }

        public List<Role> GetRoles()
        {
            return _context.Role.ToList();
        }

        public List<UserRole> GetUserRoles(int userId)
        {
            return _context.UserRole.Where(ur => ur.UserId == userId).ToList();
        }
    }
}
