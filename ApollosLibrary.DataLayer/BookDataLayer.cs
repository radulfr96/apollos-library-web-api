using ApollosLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApollosLibrary.Domain;

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

        public async Task<Book> GetBook(int id)
        {
            var book1 = _context.Books
                .Where(b => b.BookId == id)
                .Include(b => b.Genres)
                .Include(b => b.Authors)
                .FirstOrDefault();

            var book = await (from b in _context.Books
                    where b.BookId == id
                    select b).FirstOrDefaultAsync();

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

        public async Task DeleteBookAuthorRelationships(int bookId)
        {
            var book = (await _context.Books.Include(b => b.Authors).FirstOrDefaultAsync(b => b.BookId == bookId));

            if (book != null)
                book.Authors = new List<Author>();
        }

        public async Task DeleteBookGenreRelationships(int bookId)
        {
            var book = (await _context.Books.Include(b => b.Genres).FirstOrDefaultAsync(b => b.BookId == bookId));

            if (book != null)
                book.Genres = new List<Genre>();
        }
    }
}
