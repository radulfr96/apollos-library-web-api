using MyLibrary.Persistence.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.DataLayer.Contracts
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
        /// Used to add a book author relationship
        /// </summary>
        /// <param name="bookAuthor">The book author relationship to add</param>
        Task AddBookAuthor(BookAuthor bookAuthor);

        /// <summary>
        /// Used to add a books genre relationship
        /// </summary>
        /// <param name="bookGenre">The book genre relationship to add</param>
        Task AddBookGenre(BookGenre bookGenre);

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
        /// Used to get books
        /// </summary>
        /// <returns>The books</returns>
        Task<List<Book>> GetBooks();


        /// <summary>
        /// Used to get a series by its id
        /// </summary>
        /// <param name="seriesId">The id of the series to be found</param>
        /// <returns>The series with the id received</returns>
        Task<Author> GetSeries(int seriesId);

        /// <summary>
        /// Used to delete a books genre relationships
        /// </summary>
        /// <param name="bookId">The book id of the relationships to delete</param>
        void DeleteBookGenreRelationships(int bookId);

        /// <summary>
        /// Used to delete a books author relationships
        /// </summary>
        /// <param name="bookId">The book id of the relationships to delete</param>
        void DeleteBookAuthorRelationships(int bookId);
    }
}
