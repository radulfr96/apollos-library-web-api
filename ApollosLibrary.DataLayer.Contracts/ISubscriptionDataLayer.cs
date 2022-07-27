using ApollosLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.DataLayer.Contracts
{
    /// <summary>
    /// Used to handle the storage of subscriptions
    /// </summary>
    public interface ISubscriptionDataLayer
    {
        /// <summary>
        /// Used to get subscription types for the site
        /// </summary>
        /// <param name="purchasableOnly">Indicates if only purchasable subscriptions should be returned
        /// e.g. Not Staff
        /// </param>
        /// <returns>The subscription types</returns>
        Task<List<SubscriptionType>> GetSubscriptionTypes(bool purchasableOnly);

        /// <summary>
        /// Gets the users subscription information
        /// </summary>
        /// <param name="userId">The id of the user whose subscription should be returned</param>
        /// <returns>The users subscription</returns>
        Task<UserSubscription> GetUserSubscription(Guid userId);

        /// <summary>
        /// Used to get a subscription type based on the id
        /// </summary>
        /// <param name="subscriptionTypeId">The id of subscription type to be found</param>
        /// <returns>The subscription type</returns>
        Task<SubscriptionType> GetSubscriptionType(int subscriptionTypeId);

        /// <summary>
        /// The user subscription to be added
        /// </summary>
        /// <param name="userSubscription">THe user subscription to be added</param>
        Task AddUserSubscription(UserSubscription userSubscription);
    }
}
