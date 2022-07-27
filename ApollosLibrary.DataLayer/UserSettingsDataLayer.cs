using ApollosLibrary.DataLayer.Contracts;
using ApollosLibrary.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.DataLayer
{
    public class UserSettingsDataLayer : IUserSettingsDataLayer
    {
        private readonly ApollosLibraryContext _context;

        public UserSettingsDataLayer(ApollosLibraryContext context)
        {
            _context = context;
        }

        public async Task<List<UserBudgetSetting>> GetUserBudgetSettings(Guid userId)
        {
            return await _context.UserBudgetSettings.Where(u => u.UserId == userId).ToListAsync();
        }

        public async Task<UserBudgetSetting> GetUserBudgetSetting(Guid userId, int year)
        {
            return await _context.UserBudgetSettings.FirstOrDefaultAsync(u => u.UserId == userId && u.Year == year);
        }
    }
}
