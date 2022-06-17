using ApollosLibrary.Domain;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.DataLayer.Contracts
{
    /// <summary>
    /// Used to handle the storage of books
    /// </summary>
    public interface IBookDataLayer
    {
        /// <summary>
        /// Used to add a book
        /// </summary>
        /// <param name="book">the book to add</param>
        Task AddBook(Book book);

        /// <summary>
        /// Used to add a book historical record
        /// </summary>
        /// <param name="record">the book record to add</param>
        Task AddBookRecord(BookRecord record);

        /// <summary>
        /// Used to get a book by its ISBN
        /// </summary>
        /// <param name="isbn">The ISBN of the book to be found</param>
        /// <returns>The book with the ISBN received</returns>
        Task<Book> GetBookByISBN(string isbn);

        /// <summary>
        /// Used to get a book by its eISBN
        /// </summary>
        /// <param name="isbn">The eISBN of the book to be found</param>
        /// <returns>The book with the eISBN received</returns>
        Task<Book> GetBookByeISBN(string eisbn);

        /// <summary>
        /// Used to get a book by its id
        /// </summary>
        /// <param name="id">The id of the book to be found</param>
        /// <returns>The book with the id received</returns>
        Task<Book> GetBook(int id);

        /// <summary>
        /// Used to get a book record by its record id
        /// </summary>
        /// <param name="recordId">The id of the book record to be found</param>
        /// <returns>The book record with the record received</returns>
        Task<BookRecord> GetBookRecord(int recordId);

        /// <summary>
        /// Used to get books
        /// </summary>
        /// <returns>The books</returns>
        Task<List<Book>> GetBooks();

        /// <summary>
        /// Used to get a series by its id
        /// </summary>
        /// <param name="seriesId">The id of the series to be found</param>
        /// <returns>The series with the id received</returns>
        Task<Series> GetSeries(int seriesId);

        /// <summary>
        /// Used to delete a books genre relationships
        /// </summary>
        /// <param name="bookId">The book id of the relationships to delete</param>
        Task DeleteBookGenreRelationships(int bookId);

        /// <summary>
        /// Used to delete a books author relationships
        /// </summary>
        /// <param name="bookId">The book id of the relationships to delete</param>
        Task DeleteBookAuthorRelationships(int bookId);

        /// <summary>
        /// Used to delete a books series relationships
        /// </summary>
        /// <param name="bookId">The book id of the relationships to delete</param>
        Task DeleteBookSeriesRelationships(int bookId);

        /// <summary>
        /// USed to delete a book from the database
        /// </summary>
        /// <param name="bookId">THe id of the book</param>
        Task DeleteBook(int bookId);
    }
}
