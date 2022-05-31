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
        Task<List<SubscriptionType>> GetSubscriptionTypes(bool purchasableOnly);

        Task<UserSubscription> GetUserSubscription(Guid userId);

        Task AddUserSubscription(UserSubscription userSubscription);
    }
}
