using ApollosLibrary.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Subscriptions.Queries.GetUserSubscriptionDetailsQuery
{
    public class GetUserSubscriptionDetailsQueryDto
    {
        public SubscriptionTypeEnum SubscriptionType { get; set; }
        public DateTime? Expiry { get; set; }
    }
}
