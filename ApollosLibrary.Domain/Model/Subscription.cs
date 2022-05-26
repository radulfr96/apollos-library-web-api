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
        public DateTime ExpiryDate { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
