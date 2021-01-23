using MyLibrary.Persistence.Model;
using MyLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MyLibrary.DataLayer
{
    public class RoleDataLayer : IRoleDataLayer
    {
        private MyLibraryContext _context;

        public RoleDataLayer(MyLibraryContext context)
        {
            _context = context;
        }

        public async Task AddRole(Role role)
        {
            await _context.AddAsync(role);
        }

        public async Task<Role> GetRole(int roleId)
        {
            return await _context.Role.FirstOrDefaultAsync(r => r.RoleId == roleId);
        }

        public async Task<List<Role>> GetRoles()
        {
            return await _context.Role.ToListAsync();
        }

        public async Task<List<UserRole>> GetUserRoles(int userId)
        {
            return await _context.UserRole.Where(ur => ur.UserId == userId).ToListAsync();
        }
    }
}
