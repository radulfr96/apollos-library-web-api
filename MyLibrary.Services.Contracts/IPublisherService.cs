using MyLibrary.Common.Requests;
using MyLibrary.Common.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.Services.Contracts
{
    public interface IPublisherService
    {
        /// <summary>
        /// Used to add a new publisher
        /// </summary>
        /// <param name="request">request containing the publisher information</param>
        /// <returns>The response indicating the result</returns>
        BaseResponse AddPublisher(AddPublisherRequest request);

        /// <summary>
        /// Used to get a publisher
        /// </summary>
        /// <param name="id">The id of the publisher to be found</param>
        /// <returns>The response indicating the result</returns>
        GetPublisherResponse GetPublisher(int id);

        /// <summary>
        /// Used to retreive all publishers
        /// </summary>
        /// <returns>The response indicating the result</returns>
        GetPublishersResponse GetPublishers();

        /// <summary>
        /// Used to update a publisher
        /// </summary>
        /// <param name="request">Contains the publisher information to be updated</param>
        /// <returns>The response indicating the result</returns>
        BaseResponse UpdatePublisher(UpdatePublisherRequest request);

        /// <summary>
        /// Used to delete a publisher
        /// </summary>
        /// <param name="id">The id of the publisher to be deleted</param>
        /// <returns>The response indicating the result</returns>
        BaseResponse DeletePublisher(int id);
    }
}
