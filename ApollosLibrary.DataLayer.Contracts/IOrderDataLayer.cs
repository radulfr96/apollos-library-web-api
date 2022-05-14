using ApollosLibrary.Domain;
using ApollosLibrary.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.DataLayer.Contracts
{
    /// <summary>
    /// Used to handle the storage of orders
    /// </summary>
    public interface IOrderDataLayer
    {
        /// <summary>
        /// Used to the the order received
        /// </summary>
        /// <param name="order">The order to be added</param>
        Task AddOrder(Order order);

        /// <summary>
        /// Used to get a order by its id
        /// </summary>
        /// <param name="id">The id of the order to be found</param>
        /// <returns>The order with the id received</returns>
        Task<Order> GetOrder(int id);

        /// <summary>
        /// Used to get all order
        /// </summary>
        /// <returns>The list of order</returns>
        Task<List<Order>> GetOrders(Guid userId);

        /// <summary>
        /// Used to remove an order from the database
        /// </summary>
        /// <param name="order">The order to be deleted</param>
        void DeleteOrder(Order order);

        /// <summary>
        /// Used to delete an orders items
        /// </summary>
        /// <param name="orderId">Id of the order to remove items from</param>
        Task DeleteOrderItems(int orderId);
    }
}
