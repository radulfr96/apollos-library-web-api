using ApollosLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.DataLayer.Contracts
{
    /// <summary>
    /// Used to handle the storage of libraries and its entries
    /// </summary>
    public interface ILibraryDataLayer
    {
        /// <summary>
        /// Used to create the library received
        /// </summary>
        /// <param name="library">The library to be added</param>
        Task AddLibrary(Library library);

        /// <summary>
        /// Used to add a library entry
        /// </summary>
        /// <param name="entry">The entry to be added</param>
        Task AddLibraryEntry(LibraryEntry entry);

        /// <summary>
        /// Used to get the id of the library for the user
        /// </summary>
        /// <param name="userId">The user id to look up the library</param>
        /// <returns>The id of the users library</returns>
        Task<int?> GetLibraryIdByUserId(Guid userId);

        /// <summary>
        /// Used to get a library entry by its id
        /// </summary>
        /// <param name="id">The id of the library entry to be found</param>
        /// <returns>The library with the id received</returns>
        Task<LibraryEntry> GetLibraryEntry(int id);

        /// <summary>
        /// Used to get book ids in a users library by its id
        /// </summary>
        /// <param name="id">The id of the library vontaining the books</param>
        /// <returns>The books in the library</returns>
        Task<List<Book>> GetLibraryBooks(int libraryId);

        /// <summary>
        /// Used to get a library entry by book id and library id
        /// </summary>
        /// <param name="bookId">The id of the book in the entry</param>
        /// <param name="libraryId">The id of the library for the entry</param>
        /// <returns>The library with the id received</returns>
        Task<LibraryEntry> GetLibraryEntry(int libraryId, int bookId);

        /// <summary>
        /// Used to get all genres
        /// </summary>
        /// <param name="libraryId">The id of the library to returned</param>
        /// <returns>The list of genres</returns>
        Task<List<LibraryEntry>> GetLibraryEntriesByUserId(Guid userId);

        /// <summary>
        /// Used to get the library object for the user
        /// </summary>
        /// <param name="libraryId">Id of the library to be found</param>
        /// <returns>The library required</returns>
        Task<Library> GetLibrary(int libraryId);

        /// <summary>
        /// Used to remove a library entry from the database
        /// </summary>
        /// <param name="id">The id of the library entry to be deleted</param>
        Task DeleteLibraryEntry(int id);
    }
}
