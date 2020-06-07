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

        public List<Book> GetBooks()
        {
            var books = _context.Book.ToList();

            foreach (var book in books)
            {
                book.FictionType = _context.FictionType.FirstOrDefault(f => f.TypeId == book.FictionTypeId);

                book.FormType = _context.FormType.FirstOrDefault(p => p.TypeId == book.FormTypeId);
            }    

            return books;
        }
    }
}
