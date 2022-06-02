using ApollosLibrary.Application.Common.Enums;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.UnitOfWork.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.WebApi.Filters
{
    public class SubscriptionFilterAttribute : ActionFilterAttribute
    {
        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var svc = context.HttpContext.RequestServices;
            ISubscriptionUnitOfWork subscriptionUnitOfWork = svc.GetService(typeof(ISubscriptionUnitOfWork)) as ISubscriptionUnitOfWork;
            IUserService userService = svc.GetService(typeof(IUserService)) as IUserService;

            var sub = await subscriptionUnitOfWork.SubscriptionDataLayer.GetUserSubscription(userService.GetUserId());

            if (sub == null 
                || (
                sub.Subscription.SubscriptionTypeId != (int)SubscriptionTypeEnum.Staff
                && sub.Subscription.SubscriptionTypeId != (int)SubscriptionTypeEnum.Individual
                && sub.Subscription.SubscriptionTypeId != (int)SubscriptionTypeEnum.Family
                ))
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
            else
            {
                await next();
            }
        }
    }
}
