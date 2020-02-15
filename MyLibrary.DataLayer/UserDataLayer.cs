using Microsoft.EntityFrameworkCore;
using MyLibrary.Data.Model;
using MyLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyLibrary.DataLayer
{
    public class UserDataLayer : IUserDataLayer
    {
        private MyLibraryContext _context;

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
            return (
            from u in _context.User
            where u.UserId == id
            select new User()
            {
                CreatedBy = u.CreatedBy,
                CreatedDate = u.CreatedDate,
                IsActive = u.IsActive,
                ModifiedBy = u.ModifiedBy,
                ModifiedDate = u.ModifiedDate,
                Password = u.Password,
                Salter = u.Salter,
                UserId = u.UserId,
                Username = u.Username,
                UserRole =
                (from ur in _context.UserRole
                 join r in _context.Role on ur.RoleId equals r.RoleId
                 select new UserRole()
                 {
                     RoleId = r.RoleId,
                     UserId = u.UserId,
                     Role = new Role()
                     {
                         Name = r.Name,
                         RoleId = r.RoleId
                     }
                 }).ToList()
            }).FirstOrDefault();
        }

        public User GetUserByUsername(string username)
        {
            return (
            from u in _context.User
            where u.Username == username
            select new User()
            {
                CreatedBy = u.CreatedBy,
                CreatedDate = u.CreatedDate,
                IsActive = u.IsActive,
                ModifiedBy = u.ModifiedBy,
                ModifiedDate = u.ModifiedDate,
                Password = u.Password,
                Salter = u.Salter,
                UserId = u.UserId,
                Username = u.Username,
                UserRole =
                (from ur in _context.UserRole
                 join r in _context.Role on ur.RoleId equals r.RoleId
                 select new UserRole()
                 {
                     RoleId = r.RoleId,
                     UserId = u.UserId,
                     Role = new Role()
                     {
                         Name = r.Name,
                         RoleId = r.RoleId
                     }
                 }).ToList()
            }).FirstOrDefault();
        }

        public List<User> GetUsers()
        {
            return (
                from u in _context.User
                select new User()
                {
                    CreatedBy = u.CreatedBy,
                    CreatedDate = u.CreatedDate,
                    IsActive = u.IsActive,
                    ModifiedBy = u.ModifiedBy,
                    ModifiedDate = u.ModifiedDate,
                    Password = u.Password,
                    Salter = u.Salter,
                    UserId = u.UserId,
                    Username = u.Username,
                    UserRole =
                    (from ur in _context.UserRole
                     join r in _context.Role on ur.RoleId equals r.RoleId
                     select new UserRole()
                     {
                         RoleId = r.RoleId,
                         UserId = u.UserId,
                         Role = new Role()
                         {
                             Name = r.Name,
                             RoleId = r.RoleId
                         }
                     }).ToList()
                }).ToList();
        }
    }
}