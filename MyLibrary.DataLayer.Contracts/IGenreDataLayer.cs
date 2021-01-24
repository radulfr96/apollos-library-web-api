using MyLibrary.Persistence.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.DataLayer.Contracts
{
    /// <summary>
    /// Used to handle the storage of genres
    /// </summary>
    public interface IGenreDataLayer
    {
        /// <summary>
        /// Used to the the genre received
        /// </summary>
        /// <param name="genre">The genre to be added</param>
        Task AddGenre(Genre genre);

        /// <summary>
        /// Used to get a genre by its id
        /// </summary>
        /// <param name="id">The id of the genre to be found</param>
        /// <returns>The genre with the id received</returns>
        Task<Genre> GetGenre(int id);

        /// <summary>
        /// Used to get all genres
        /// </summary>
        /// <returns>The list of genres</returns>
        Task<List<Genre>> GetGenres();

        /// <summary>
        /// Used to remove a genre from the database
        /// </summary>
        /// <param name="id">The id of the genre to be deleted</param>
        Task DeleteGenre(int id);
    }
}
