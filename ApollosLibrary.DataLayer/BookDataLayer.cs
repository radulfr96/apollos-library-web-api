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
        private readonly ApollosLibraryContext _context;

        public BookDataLayer(ApollosLibraryContext context)
        {
            _context = context;
        }

        public async Task AddBook(Book book)
        {
            await _context.Books.AddAsync(book);
        }

        public async Task AddBookRecord(BookRecord record)
        {
            await _context.BookRecords.AddAsync(record);
        }

        public async Task<Book> GetBook(int id)
        {
            return await _context.Books
                .Where(b => b.BookId == id && !b.IsDeleted)
                .Include(b => b.Genres)
                .Include(b => b.Authors)
                .Include(b => b.Series)
                .FirstOrDefaultAsync();
        }

        public async Task<Book> GetBookByeISBN(string eisbn)
        {
            return await _context.Books
                .Where(b => b.EIsbn == eisbn)
                .Include(b => b.Genres)
                .Include(b => b.Authors)
                .Include(b => b.Series)
                .FirstOrDefaultAsync();
        }

        public async Task<Book> GetBookByISBN(string isbn)
        {
            return await _context.Books
                .Where(b => b.Isbn == isbn)
                .Include(b => b.Genres)
                .Include(b => b.Authors)
                .Include(b => b.Series)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Book>> GetBooks()
        {
            var books = await _context.Books.Where(b => !b.IsDeleted).ToListAsync();

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

        public async Task DeleteBookSeriesRelationships(int bookId)
        {
            var book = (await _context.Books.Include(b => b.Series).FirstOrDefaultAsync(b => b.BookId == bookId));

            if (book != null)
                book.Series = new List<Series>();
        }

        public async Task DeleteBook(int bookId)
        {
            var book = (await _context.Books.Include(b => b.Genres).FirstOrDefaultAsync(b => b.BookId == bookId));

            if (book != null)
                _context.Books.Remove(book);
        }
    }
}
