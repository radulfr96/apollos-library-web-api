using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Domain.Model
{
    # nullable disable

    public class UserSubscription
    {
        [Key]
        public int UserSubscrptionId { get; set; }
        public Guid UserId { get; set; }
        public Guid SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }
    }
}
