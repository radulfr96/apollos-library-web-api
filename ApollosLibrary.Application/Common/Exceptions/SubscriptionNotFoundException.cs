using ApollosLibrary.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Common.Exceptions
{
    internal class SubscriptionNotFoundException : NotFoundException
    {
        public SubscriptionNotFoundException(Guid userId) : base(ErrorCodeEnum.SubscriptionNotFound, $"Subscription for user with id [{userId}] not found", null)
        {
        }
    }
}
