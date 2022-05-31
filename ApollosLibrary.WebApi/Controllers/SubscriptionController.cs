using ApollosLibrary.Application.Subscriptions.Queries.GetSubscriptionTypesQuery;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;
using System.IO;
using System;
using Microsoft.Build.Framework;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Subscriptions.Commands.StripeSubCreatedCommand;

namespace ApollosLibrary.WebApi.Controllers
{
    /// <summary>
    /// Used to manage a users subscription
    /// </summary>
    [Route("api/[controller]")]
    public class SubscriptionController : BaseApiController
    {
        private readonly IMediator _mediatr;
        private readonly IUserService _userService;

        public SubscriptionController(
            IConfiguration configuration
            , IMediator mediatr
            , IUserService userService
            ) : base(configuration)
        {
            _mediatr = mediatr;
            _userService = userService;
            StripeConfiguration.ApiKey = configuration.GetSection("Stripe").GetSection("APIKey").Value;
        }

        /// <summary>
        /// Used to get subscriptions that are purchasable by users
        /// </summary>
        /// <returns>The subscriptions</returns>
        [HttpGet("")]
        public async Task<GetSubsriptionTypesQueryDto> GetPaidSubscriptions()
        {
            return await _mediatr.Send(new GetSubscriptionTypesQuery()
            {
                PurchasableOnly = true,
            });
        }

        [HttpGet("subscription")]
        public async Task<> GetUserSubscription()
        {

        }

        public class CreateCheckoutRequest
        {
            public string ProductId { get; set; }
        }

        public class CreateCheckoutResponse
        {
            public string CheckoutURL { get; set; }
        }

        /// <summary>
        /// Used to start a checkout section for a subscription
        /// </summary>
        /// <param name="request">Request with the Stripe product id of the subscription</param>
        /// <returns>Result</returns>
        [HttpPost("create-checkout-session")]
        public async Task<CreateCheckoutResponse> CreateCheckout([FromBody] CreateCheckoutRequest request)
        {
            var domain = _config.GetRequiredSection("FrontEndURL").Value;

            var priceOptions = new PriceListOptions
            {
                Product = request.ProductId,
            };
            var priceService = new PriceService();
            StripeList<Price> prices = await priceService.ListAsync(priceOptions);

            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = prices.Data[0].Id,
                        Quantity = 1,
                    },
                },
                SubscriptionData = new SessionSubscriptionDataOptions()
                {
                    Metadata = new Dictionary<string, string>(),
                },
                Mode = "subscription",
                SuccessUrl = domain + "/subscriptions?success=true&session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = domain + "/subscriptions?canceled=true",
            };

            options.SubscriptionData.Metadata.Add("userId", _userService.GetUserId().ToString());
            var service = new SessionService();
            Session session = await service.CreateAsync(options);
            return new CreateCheckoutResponse()
            {
                CheckoutURL = session.Url,
            };
        }

        public class CreatePortalSessionRequest
        {
            public string SessionId { get; set; }
        }

        public class CreatePortalSessionResponse
        {
            public string PortalURL { get; set; }
        }

        [HttpPost("create-portal-session")]
        public async Task<CreatePortalSessionResponse> CreatePortalSession([FromBody] CreatePortalSessionRequest request)
        {
            // For demonstration purposes, we're using the Checkout session to retrieve the customer ID.
            // Typically this is stored alongside the authenticated user in your database.
            var checkoutService = new SessionService();
            var checkoutSession = checkoutService.Get(request.SessionId);

            // This is the URL to which your customer will return after
            // they are done managing billing in the Customer Portal.
            var returnUrl = _config.GetRequiredSection("FrontEndURL").Value;

            var options = new Stripe.BillingPortal.SessionCreateOptions
            {
                Customer = checkoutSession.CustomerId,
                ReturnUrl = returnUrl,
            };
            var service = new Stripe.BillingPortal.SessionService();
            var session = service.Create(options);

            return new CreatePortalSessionResponse()
            {
                PortalURL = session.Url,
            };
        }

        [AllowAnonymous]
        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            // Replace this endpoint secret with your endpoint's unique secret
            // If you are testing with the CLI, find the secret by running 'stripe listen'
            // If you are using an endpoint defined with the API or dashboard, look in your webhook settings
            // at https://dashboard.stripe.com/webhooks
            string endpointSecret = _config.GetSection("Stripe").GetSection("EndpointSecret").Value;

            var customerService = new CustomerService();
            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);
                var signatureHeader = Request.Headers["Stripe-Signature"];
                stripeEvent = EventUtility.ConstructEvent(json,
                        signatureHeader, endpointSecret);
                if (stripeEvent.Type == Events.CustomerSubscriptionDeleted)
                {
                    var subscription = stripeEvent.Data.Object as Subscription;
                    subscription.Customer = customerService.Get(subscription.CustomerId);
                    _logger.Warn("A subscription was canceled.", subscription.Id);
                    // Then define and call a method to handle the successful payment intent.
                    await _mediatr.Send(new StripeSubUpdateCommand()
                    {
                        StripeSubscription = subscription,
                    });
                }
                else if (stripeEvent.Type == Events.CustomerSubscriptionUpdated)
                {
                    var subscription = stripeEvent.Data.Object as Subscription;
                    subscription.Customer = customerService.Get(subscription.CustomerId);
                    _logger.Debug("A subscription was updated.", subscription.Id);
                    // Then define and call a method to handle the successful payment intent.
                    await _mediatr.Send(new StripeSubUpdateCommand()
                    {
                        StripeSubscription = subscription,
                    });
                }
                else if (stripeEvent.Type == Events.CustomerSubscriptionTrialWillEnd)
                {
                    var subscription = stripeEvent.Data.Object as Subscription;
                    subscription.Customer = customerService.Get(subscription.CustomerId);
                    _logger.Debug("A subscription trial will end", subscription.Id);
                    // Then define and call a method to handle the successful payment intent.
                    // handleSubscriptionUpdated(subscription);
                }
                else
                {
                    _logger.Debug("Unhandled event type: {0}", stripeEvent.Type);
                }
                return Ok();
            }
            catch (StripeException e)
            {
                _logger.Error("Error processing webhook: {0}", e.Message);
                return BadRequest();
            }
        }
    }
}
