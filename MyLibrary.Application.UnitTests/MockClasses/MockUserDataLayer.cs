using MyLibrary.DataLayer.Contracts;
using MyLibrary.Persistence.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Services.XUnitTestProject.MockClasses
{
    public class MockUserDataLayer : IUserDataLayer
    {
        public List<User> Users { get; set; }

        public MockUserDataLayer()
        {
            Users = new List<User>();
        }

        public async Task AddUser(User user)
        {
            user.UserId = Users.Count() + 1;
            Users.Add(user);
        }

        public async Task<User> GetUser(int id)
        {
            return Users.FirstOrDefault(u => u.UserId == id);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return Users.FirstOrDefault(u => u.Username == username);
        }

        public async Task<List<User>> GetUsers()
        {
            return Users;
        }

        public void ClearUserRoles(User user)
        {
            user.UserRoles.Clear();
        }
    }
}
