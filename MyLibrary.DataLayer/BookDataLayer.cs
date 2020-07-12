using MyLibrary.Data.Model;
using MyLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLibrary.DataLayer
{
    public class BookDataLayer : IBookDataLayer
    {
        private MyLibraryContext _context;

        public BookDataLayer(MyLibraryContext context)
        {
            _context = context;
        }

        public void AddBook(Book book)
        {
            _context.Book.Add(book);
        }

        public void AddBookAuthor(BookAuthor bookAuthor)
        {
            _context.BookAuthor.Add(bookAuthor);
        }

        public void AddBookGenre(BookGenre bookGenre)
        {
            _context.BookGenre.Add(bookGenre);
        }

        public Book GetBook(int id)
        {
            var book = (from b in _context.Book
                    where b.BookId == id
                    select b).FirstOrDefault();

            if (book != null)
            {
                book.BookGenre = (from bg in _context.BookGenre
                                  where bg.BookId == id
                                  select bg).ToList();

                book.BookAuthor = (from ba in _context.BookAuthor
                                   where ba.BookId == id
                                   select ba).ToList();
            }

            return book;
        }

        public Book GetBookByeISBN(string eisbn)
        {
            return (from b in _context.Book
                    where b.EIsbn == eisbn
                    select b).FirstOrDefault();
        }

        public Book GetBookByISBN(string isbn)
        {
            return (from b in _context.Book
                    where b.Isbn == isbn
                    select b).FirstOrDefault();
        }

        public List<Book> GetBooks()
        {
            var books = _context.Book.ToList();

            var fictionTypes = _context.FictionType.ToList();
            var formTypes = _context.FormType.ToList();

            foreach (var book in books)
            {
                book.FictionType = fictionTypes.FirstOrDefault(f => f.TypeId == book.FictionTypeId);

                book.FormType = formTypes.FirstOrDefault(p => p.TypeId == book.FormTypeId);
            }    

            return books;
        }

        public void DeleteBookAuthorRelationships(int bookId)
        {
            var bookAuthors = _context.BookAuthor.Where(b => b.BookId == bookId);

            _context.BookAuthor.RemoveRange(bookAuthors);
        }

        public void DeleteBookGenreRelationships(int bookId)
        {
            var bookGenres = _context.BookGenre.Where(b => b.BookId == bookId);

            _context.BookGenre.RemoveRange(bookGenres);
        }
    }
}
