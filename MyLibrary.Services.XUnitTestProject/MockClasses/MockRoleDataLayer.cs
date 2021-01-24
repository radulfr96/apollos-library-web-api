
using MyLibrary.Application.Common.Enums;
using MyLibrary.DataLayer.Contracts;
using MyLibrary.Persistence.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Services.XUnitTestProject.MockClasses
{
    public class MockRoleDataLayer : IRoleDataLayer
    {
        public List<UserRole> UserRoles { get; set; }
        public List<Role> Roles { get; set; }

        public MockRoleDataLayer()
        {
            UserRoles = new List<UserRole>();
            Roles = new List<Role>();
        }

        public Task AddRole(Role role)
        {
            throw new NotImplementedException();
        }

        public async Task<Role> GetRole(int roleId)
        {
            switch (roleId)
            {
                case (int)RoleEnum.Admin:
                    return new Role()
                    {
                        Name = "Admin",
                        RoleId = roleId
                    };
                case (int)RoleEnum.StandardUser:
                    return new Role()
                    {
                        Name = "Standard User",
                        RoleId = roleId
                    };
            }
            return null;
        }

        public async Task<List<Role>> GetRoles()
        {
            return Roles.ToList();
        }

        public async Task<List<UserRole>> GetUserRoles(int userId)
        {
            return UserRoles.Where(r => r.UserId == userId).ToList();
        }
    }
}
