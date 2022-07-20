using ApollosLibrary.Domain;
using ApollosLibrary.Domain.Enums;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.DowngradeSubscriptionJob
{
    public class Job
    {
        private readonly ApollosLibraryContext _context;
        private readonly IClock _clock;

        public Job(ApollosLibraryContext context, IClock clock)
        {
            _context = context;
            _clock = clock;
        }

        [NoAutomaticTrigger]
        public async Task Run(CancellationToken cancellationToken)
        {
            var instant = _clock.GetCurrentInstant();
            var now = instant.InUtc().LocalDateTime;
            var subscriptionsToDowngrade = await _context.Subscriptions
                                                .Where(s => 
                                                s.ExpiryDate < now 
                                                &&  s.SubscriptionTypeId != (int)SubscriptionTypeEnum.Individual
                                                && s.SubscriptionTypeId != (int)SubscriptionTypeEnum.Staff
                                                )
                                                .ToListAsync(cancellationToken);

            foreach(var subscription in subscriptionsToDowngrade)
            {
                subscription.SubscriptionTypeId = (int)SubscriptionTypeEnum.SignedUp;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
