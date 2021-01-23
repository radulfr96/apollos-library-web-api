using Microsoft.EntityFrameworkCore;
using MyLibrary.Persistence.Model;
using MyLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLibrary.DataLayer
{
    public class UserDataLayer : IUserDataLayer
    {
        private MyLibraryContext _context;

        public UserDataLayer(MyLibraryContext context)
        {
            _context = context;
        }

        public async Task AddUser(User user)
        {
            await _context.User.AddAsync(user);
        }

        public void ClearUserRoles(User user)
        {
            _context.UserRole.RemoveRange(user.UserRole);
            user.UserRole.Clear();
        }

        public async Task<User> GetUser(int id)
        {
            return await _context.User.Include("UserRole.Role").FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.User.Include("UserRole.Role").FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<List<User>> GetUsers()
        {
            return await (
                from u in _context.User
                where !u.IsDeleted
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
                     where ur.UserId == u.UserId
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
                }).ToListAsync();
        }
    }
}