using MyLibrary.Common.Requests;
using MyLibrary.Common.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.Services.Contracts
{
    /// <summary>
    /// Used to handle buisness logic related to genres
    /// </summary>
    public interface IGenreService
    {
        /// <summary>
        /// Used to add a new genre
        /// </summary>
        /// <param name="request">request containing the genre information</param>
        /// <returns>The response indicating the result</returns>
        AddGenreResponse AddGenre(AddGenreRequest request);

        /// <summary>
        /// Used to get a genre
        /// </summary>
        /// <param name="id">The id of the genre to be found</param>
        /// <returns>The response indicating the result</returns>
        GetGenreResponse GetGenre(int id);

        /// <summary>
        /// Used to retreive all genres
        /// </summary>
        /// <returns>The response indicating the result</returns>
        GetGenresResponse GetGenres();

        /// <summary>
        /// Used to update a genre
        /// </summary>
        /// <param name="request">Contains the genre information to be updated</param>
        /// <returns>The response indicating the result</returns>
        BaseResponse UpdateGenre(UpdateGenreRequest request);

        /// <summary>
        /// Used to delete a genre
        /// </summary>
        /// <param name="id">The id of the genre to be delete</param>
        /// <returns>The response indicating the result</returns>
        BaseResponse DeleteGenre(int id);
    }
}
