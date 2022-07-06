using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Domain
{
    # nullable disable
    public class Subscription
    {
        public Guid SubscriptionId { get; set; }
        public int SubscriptionTypeId { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public LocalDateTime? ExpiryDate { get; set; }
        public LocalDateTime SubscriptionDate { get; set; }
        public Guid SubscriptionAdmin { get; set; }
        public string StripeSubscriptionId { get; set; }
        public string StripeCustomerId { get; set; }
        public ICollection<UserSubscription> SubscriptionUsers { get; set; }
    }
}
