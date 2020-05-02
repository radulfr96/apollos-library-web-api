using MyLibrary.Common.Requests;
using MyLibrary.Common.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.Services.Contracts
{
    /// <summary>
    /// Used to handle buisness logic related to authors
    /// </summary>
    public interface IAuthorService
    {
        /// <summary>
        /// Used to add a new author
        /// </summary>
        /// <param name="request">request containing the author information</param>
        /// <returns>The response indicating the result</returns>
        AddAuthorResponse AddAuthor(AddAuthorRequest request);

        /// <summary>
        /// Used to get a author
        /// </summary>
        /// <param name="id">The id of the author to be found</param>
        /// <returns>The response indicating the result</returns>
        GetAuthorResponse GetAuthor(int id);

        /// <summary>
        /// Used to retreive all authors
        /// </summary>
        /// <returns>The response indicating the result</returns>
        GetAuthorsResponse GetAuthors();

        /// <summary>
        /// Used to update an author
        /// </summary>
        /// <param name="request">Contains the author information to be updated</param>
        /// <returns>The response indicating the result</returns>
        BaseResponse UpdateAuthor(UpdateAuthorRequest request);

        /// <summary>
        /// Used to delete a genre
        /// </summary>
        /// <param name="id">The id of the author to be delete</param>
        /// <returns>The response indicating the result</returns>
        BaseResponse DeleteAuthor(int id);
    }
}
