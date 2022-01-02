using Microsoft.EntityFrameworkCore;
using MyLibrary.DataLayer.Contracts;
using MyLibrary.Persistence.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            await _context.Users.AddAsync(user);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user != null)
            {
                user.UserClaims = await _context.UserClaims.Where(u => u.UserId == user.UserId).ToListAsync();
            }

            return user;
        }

        public async Task<User> GetUser(Guid id)
        {
            return await _context.Users
                                 .Include(u => u.UserClaims)
                                 .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<List<User>> GetUsers()
        {
            return await (
                from u in _context.Users
                select new User()
                {
                    CreatedBy = u.CreatedBy,
                    CreatedDate = u.CreatedDate,
                    IsActive = u.IsActive,
                    ModifiedBy = u.ModifiedBy,
                    ModifiedDate = u.ModifiedDate,
                    Password = u.Password,
                    UserId = u.UserId,
                    Username = u.Username,
                }).ToListAsync();
        }
    }
}
