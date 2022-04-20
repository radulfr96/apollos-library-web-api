using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Domain.Model
{
    #nullable disable

    public class SubscriptionType
    {
        public int SubscriptionTypeId { get; set; }
        public string SubscriptionName { get; set; }
        public decimal MonthlyRate { get; set; }
    }
}
