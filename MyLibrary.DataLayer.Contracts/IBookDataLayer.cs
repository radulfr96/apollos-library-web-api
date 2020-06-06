using MyLibrary.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.DataLayer.Contracts
{
    /// <summary>
    /// Used to handle the storage of books
    /// </summary>
    public interface IBookDataLayer
    {
        /// <summary>
        /// Used to get a book by its id
        /// </summary>
        /// <param name="id">The id of the book to be found</param>
        /// <returns>The book with the id received</returns>
        Book GetBook(int id);

        /// <summary>
        /// Used to get a books
        /// </summary>
        /// <returns>The books</returns>
        List<Book> GetBooks();
    }
}
