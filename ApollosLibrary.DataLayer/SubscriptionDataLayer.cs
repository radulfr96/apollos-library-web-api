using ApollosLibrary.DataLayer.Contracts;
using ApollosLibrary.Domain;
using ApollosLibrary.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.DataLayer
{
    public class SubscriptionDataLayer : ISubscriptionDataLayer
    {
        private readonly ApollosLibraryContext _context;

        public SubscriptionDataLayer(ApollosLibraryContext context)
        {
            _context = context;
        }

        public async Task<List<SubscriptionType>> GetSubscriptionTypes()
        {
            return await _context.SubscriptionTypes.ToListAsync();
        }

        public async Task<UserSubscription> GetUserSubscription(Guid userId)
        {
            return await _context.UserSubscriptions
                .Include(us => us.Subscription)
                .ThenInclude(s => s.SubscriptionType)
                .FirstOrDefaultAsync(us => us.UserId == userId);
        }
    }
}
