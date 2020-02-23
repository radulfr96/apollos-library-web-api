using MyLibrary.Data.Enums;
using MyLibrary.Data.Model;
using MyLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.Services.XUnitTestProject.MockClasses
{
    public class MockRoleDataLayer : IRoleDataLayer
    {
        public void AddRole(Role role)
        {
            throw new NotImplementedException();
        }

        public Role GetRole(int roleId)
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

        public List<Role> GetRoles()
        {
            throw new NotImplementedException();
        }

        public List<UserRole> GetUserRoles(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
