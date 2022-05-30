using ApollosLibrary.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Common.Exceptions
{
    public class StripeSubscriptionMissingUserIdException : BadRequestException
    {
        public StripeSubscriptionMissingUserIdException() : base(ErrorCodeEnum.StripeSubscriptionMissingUserId, "Stripe subscription missing user id", null)
        {
        }
    }
}
