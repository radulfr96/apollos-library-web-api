using MyLibrary.Data.Model;
using MyLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyLibrary.DataLayer
{
    public class UserDataLayer : IUserDataLayer, IDisposable
    {
        private MyLibraryContext _context;
        private bool disposed = false;

        public UserDataLayer(MyLibraryContext context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            _context.User.Add(user);
        }

        public User GetUser(int id)
        {
            return _context.User.FirstOrDefault(u => u.UserId == id);
        }

        public User GetUserByUsername(string username)
        {
            return _context.User.FirstOrDefault(u => u.Username == username);
        }

        public List<User> GetUsers()
        {
            return _context.User.ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                _context.Dispose();
            }

            disposed = true;
        }

        ~UserDataLayer()
        {
            Dispose(false);
        }
    }
}