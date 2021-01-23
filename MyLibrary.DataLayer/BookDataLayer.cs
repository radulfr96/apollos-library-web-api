using MyLibrary.Persistence.Model;
using MyLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MyLibrary.DataLayer
{
    public class BookDataLayer : IBookDataLayer
    {
        private MyLibraryContext _context;

        public BookDataLayer(MyLibraryContext context)
        {
            _context = context;
        }

        public async Task AddBook(Book book)
        {
            await _context.Book.AddAsync(book);
        }

        public async Task AddBookAuthor(BookAuthor bookAuthor)
        {
            await _context.BookAuthor.AddAsync(bookAuthor);
        }

        public async Task AddBookGenre(BookGenre bookGenre)
        {
            await _context.BookGenre.AddAsync(bookGenre);
        }

        public async Task<Book> GetBook(int id)
        {
            var book = await (from b in _context.Book
                    where b.BookId == id
                    select b).FirstOrDefaultAsync();

            if (book != null)
            {
                book.BookGenre = await (from bg in _context.BookGenre
                                  where bg.BookId == id
                                  select bg).ToListAsync();

                book.BookAuthor = await (from ba in _context.BookAuthor
                                   where ba.BookId == id
                                   select ba).ToListAsync();
            }

            return book;
        }

        public async Task<Book> GetBookByeISBN(string eisbn)
        {
            return await (from b in _context.Book
                    where b.EIsbn == eisbn
                    select b).FirstOrDefaultAsync();
        }

        public async Task<Book> GetBookByISBN(string isbn)
        {
            return await (from b in _context.Book
                    where b.Isbn == isbn
                    select b).FirstOrDefaultAsync();
        }

        public async Task<List<Book>> GetBooks()
        {
            var books = await _context.Book.ToListAsync();

            var fictionTypes = await _context.FictionType.ToListAsync();
            var formTypes = await _context.FormType.ToListAsync();

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
