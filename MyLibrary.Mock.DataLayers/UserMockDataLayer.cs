using MyLibrary.Data.Model;
using MyLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyLibrary.Mock.DataLayers
{
    public class UserMockDataLayer : IUserDataLayer
    {
        List<User> _users = new List<User>();

        public UserMockDataLayer(List<User> seedData)
        {
            _users = seedData;
        }

        public void AddUser(User user)
        {
            _users.Add(user);
        }

        public User GetUser(int id)
        {
            return _users.FirstOrDefault(u => u.UserId == id);
        }

        public User GetUserByUsername(string username)
        {
            return _users.FirstOrDefault(u => u.Username == username);
        }

        public List<User> GetUsers()
        {
            return _users;
        }

        public void Save() {}
    }
}
