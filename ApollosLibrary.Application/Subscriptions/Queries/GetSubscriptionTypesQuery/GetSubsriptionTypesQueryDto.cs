using ApollosLibrary.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Subscriptions.Queries.GetSubscriptionTypesQuery
{
    public class GetSubsriptionTypesQueryDto
    {
        public List<SubscriptionTypeDTO> SubscriptionTypes { get; set; } = new List<SubscriptionTypeDTO>();
    }

    public class SubscriptionTypeDTO
    {
        public SubscriptionTypeEnum SubscriptionType { get; set; }
        public string SubscriptionName { get; set; }
        public int MaxUsers { get; set; }
        public decimal Cost { get; set; }
        public string PriceId { get; set; }
        public string Description { get; set; }
    }
}
