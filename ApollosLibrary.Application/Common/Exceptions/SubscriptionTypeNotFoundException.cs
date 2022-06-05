using ApollosLibrary.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Common.Exceptions
{
    internal class SubscriptionTypeNotFoundException : NotFoundException
    {
        public SubscriptionTypeNotFoundException() : base(ErrorCodeEnum.SubscriptionTypeNotFound, "Unable to find subscription type", null)
        {
        }
    }
}
