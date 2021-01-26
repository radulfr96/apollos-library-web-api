using MyLibrary.Contracts.UnitOfWork;
using MyLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Services.XUnitTestProject.MockClasses
{
    public class MockUserUnitOfWork : IUserUnitOfWork
    {
        public IUserDataLayer MockUserDataLayer { get; set; }

        public IRoleDataLayer MockRoleDataLayer { get; set; }

        public IUserDataLayer UserDataLayer
        {
            get
            {
                return MockUserDataLayer;
            }
        }

        public IRoleDataLayer RoleDataLayer 
        {
            get
            {
                return MockRoleDataLayer;
            }
        }

        public async Task Begin()
        {
            
        }

        public async Task Commit()
        {
            
        }

        public async Task Save()
        {
            
        }
    }
}
