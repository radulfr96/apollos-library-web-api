﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Domain
{
    #nullable disable

    public class SubscriptionType
    {
        [Key]
        public int SubscriptionTypeId { get; set; }
        public string SubscriptionName { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal MonthlyRate { get; set; }
        public string StripeProductId { get; set; }
        public bool Purchasable { get; set; }
        public bool IsAvailable { get; set; }
        public int MaxUsers { get; set; }
        public string Description { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; }
    }
}
