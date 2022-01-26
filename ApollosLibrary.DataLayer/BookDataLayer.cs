using ApollosLibrary.Persistence.Model;
using ApollosLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ApollosLibrary.DataLayer
{
    public class BookDataLayer : IBookDataLayer
    {
        private ApollosLibraryContext _context;

        public BookDataLayer(ApollosLibraryContext context)
        {
            _context = context;
        }

        public async Task AddBook(Book book)
        {
            await _context.Books.AddAsync(book);
        }

        public async Task AddBookAuthor(BookAuthor bookAuthor)
        {
            await _context.BookAuthors.AddAsync(bookAuthor);
        }

        public async Task AddBookGenre(BookGenre bookGenre)
        {
            await _context.BookGenres.AddAsync(bookGenre);
        }

        public async Task<Book> GetBook(int id)
        {
            var book = await (from b in _context.Books
                    where b.BookId == id
                    select b).FirstOrDefaultAsync();

            if (book != null)
            {
                book.BookGenres = await (from bg in _context.BookGenres
                                  where bg.BookId == id
                                  select bg).ToListAsync();

                book.BookAuthors = await (from ba in _context.BookAuthors
                                   where ba.BookId == id
                                   select ba).ToListAsync();
            }

            return book;
        }

        public async Task<Book> GetBookByeISBN(string eisbn)
        {
            return await (from b in _context.Books
                    where b.EIsbn == eisbn
                    select b).FirstOrDefaultAsync();
        }

        public async Task<Book> GetBookByISBN(string isbn)
        {
            return await (from b in _context.Books
                    where b.Isbn == isbn
                    select b).FirstOrDefaultAsync();
        }

        public async Task<List<Book>> GetBooks()
        {
            var books = await _context.Books.ToListAsync();

            var fictionTypes = await _context.FictionTypes.ToListAsync();
            var formTypes = await _context.FormTypes.ToListAsync();

            foreach (var book in books)
            {
                book.FictionType = fictionTypes.FirstOrDefault(f => f.TypeId == book.FictionTypeId);

                book.FormType = formTypes.FirstOrDefault(p => p.TypeId == book.FormTypeId);
            }    

            return books;
        }

        public async Task<Series> GetSeries(int seriesId)
        {
            return await (from s in _context.Series
                          where s.SeriesId == seriesId
                          select s).FirstOrDefaultAsync();
        }

        public void DeleteBookAuthorRelationships(int bookId)
        {
            var bookAuthors = _context.BookAuthors.Where(b => b.BookId == bookId);

            _context.BookAuthors.RemoveRange(bookAuthors);
        }

        public void DeleteBookGenreRelationships(int bookId)
        {
            var bookGenres = _context.BookGenres.Where(b => b.BookId == bookId);

            _context.BookGenres.RemoveRange(bookGenres);
        }
    }
}
