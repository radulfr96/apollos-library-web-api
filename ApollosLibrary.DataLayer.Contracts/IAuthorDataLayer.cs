using ApollosLibrary.Domain;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.DataLayer.Contracts
{
    /// <summary>
    /// Used to handle the storage of authors
    /// </summary>
    public interface IAuthorDataLayer
    {
        /// <summary>
        /// Used to the the author received
        /// </summary>
        /// <param name="author">The author to be added</param>
        Task AddAuthor(Author author);

        /// <summary>
        /// Used to get a author by its id
        /// </summary>
        /// <param name="id">The id of the author to be found</param>
        /// <returns>The author with the id received</returns>
        Task<Author> GetAuthor(int id);

        /// <summary>
        /// Used to get all authors
        /// </summary>
        /// <returns>The list of authors</returns>
        Task<List<Author>> GetAuthors();

        /// <summary>
        /// Used to remove an author from the database
        /// </summary>
        /// <param name="id">The id of the author to be deleted</param>
        Task DeleteAuthor(Author author);
    }
}
