using MyLibrary.Common.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.Services.Contracts
{
    /// <summary>
    /// Used to handle buisness logic related to books
    /// </summary>
    public interface IBookService
    {
        /// <summary>
        /// Used to get a book
        /// </summary>
        /// <param name="id">The id of the book to be found</param>
        /// <returns>The response indicating the result</returns>
        GetAuthorResponse GetBook(int id);
    }
}
