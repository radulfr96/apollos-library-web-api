using MyLibrary.Data.Model;
using MyLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLibrary.Services.XUnitTestProject.MockClasses
{
    public class MockUserDataLayer : IUserDataLayer
    {
        public List<User> Users { get; set; }

        public MockUserDataLayer()
        {
            Users = new List<User>();
        }

        public void AddUser(User user)
        {
            user.UserId = Users.Count() + 1;
            Users.Add(user);
        }

        public User GetUser(int id)
        {
            return Users.FirstOrDefault(u => u.UserId == id);
        }

        public User GetUserByUsername(string username)
        {
            return Users.FirstOrDefault(u => u.Username == username);
        }

        public List<User> GetUsers()
        {
            return Users;
        }

        public void ClearUserRoles(User user)
        {
            user.UserRole.Clear();
        }
    }
}
