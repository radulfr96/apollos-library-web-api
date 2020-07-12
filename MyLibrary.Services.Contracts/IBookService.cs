using MyLibrary.Common.Requests;
using MyLibrary.Common.Requests.Book;
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
        /// Used to add a book
        /// </summary>
        /// <param name="request">the request with the book information</param>
        /// <returns>the response indicating the result and the book id if added</returns>
        AddBookResponse AddBook(AddBookRequest request);

        /// <summary>
        /// Used to update a book
        /// </summary>
        /// <param name="request">the request with the book information</param>
        /// <returns>the response indicating the result and the book id if added</returns>
        BaseResponse UpdateBook(UpdateBookRequest request);

        /// <summary>
        /// Used to get a book
        /// </summary>
        /// <param name="id">The id of the book to be found</param>
        /// <returns>The response indicating the result</returns>
        GetBookResponse GetBook(int id);

        /// <summary>
        /// Used to get a books
        /// </summary>
        /// <returns>The response indicating the result</returns>
        GetBooksResponse GetBooks();
    }
}
