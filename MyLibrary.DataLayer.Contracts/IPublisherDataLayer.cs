using MyLibrary.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.DataLayer.Contracts
{
    /// <summary>
    /// Used to hanlde storage of publishers
    /// </summary>
    public interface IPublisherDataLayer
    {
        /// <summary>
        /// Used to the the publisher received
        /// </summary>
        /// <param name="genre">The publisher to be added</param>
        void AddPublisher(Publisher publisher);

        /// <summary>
        /// Used to get a publisher by its id
        /// </summary>
        /// <param name="id">The id of the publisher to be found</param>
        /// <returns>The publisher with the id received</returns>
        Publisher GetPublisher(int id);

        /// <summary>
        /// Used to get all publishers
        /// </summary>
        /// <returns>The list of publishers</returns>
        List<Publisher> GetPublishers();
    }
}
