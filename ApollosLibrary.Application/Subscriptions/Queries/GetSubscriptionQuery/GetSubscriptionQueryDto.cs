using ApollosLibrary.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Subscriptions.Queries.GetSubscriptionQuery
{
    public class GetSubscriptionQueryDto
    {
        public SubscriptionTypeEnum SubscriptionType { get; set; }
        public string SubscriptionName { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime? Expiry { get; set; }
        public bool SubscriptionAdmin { get; set; }

        public List<SubscriptionUserDTO> SubscriptionUsers = new List<SubscriptionUserDTO>();
    }

    public class SubscriptionUserDTO
    {
        public string Email { get; set; }
        public Guid UserId { get; set; }
    }
}
