using ApollosLibrary.Domain;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.DataLayer.Contracts
{
    /// <summary>
    /// Used to hanlde storage of Businesss
    /// </summary>
    public interface IBusinessDataLayer
    {
        /// <summary>
        /// Used to the the Business received
        /// </summary>
        /// <param name="genre">The Business to be added</param>
        Task AddBusiness(Business Business);

        /// <summary>
        /// Used to get a Business by its id
        /// </summary>
        /// <param name="id">The id of the Business to be found</param>
        /// <returns>The Business with the id received</returns>
        Task<Business> GetBusiness(int id);

        /// <summary>
        /// Used to get all Businesss
        /// </summary>
        /// <returns>The list of Businesss</returns>
        Task<List<Business>> GetBusinesss();
    }
}
