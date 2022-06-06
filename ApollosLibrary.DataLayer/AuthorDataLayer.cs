using Microsoft.EntityFrameworkCore;
using ApollosLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApollosLibrary.Domain;

namespace ApollosLibrary.DataLayer
{
    public class AuthorDataLayer : IAuthorDataLayer
    {
        private readonly ApollosLibraryContext _context;

        public AuthorDataLayer(ApollosLibraryContext context)
        {
            _context = context;
        }

        public async Task AddAuthor(Author author)
        {
            await _context.Authors.AddAsync(author);
        }

        public async Task AddAuthorRecord(AuthorRecord record)
        {
            await _context.AuthorRecords.AddAsync(record);
        }

        public async Task DeleteAuthor(Author author)
        {
            await Task.FromResult(_context.Authors.Remove(author));
        }

        public async Task<Author> GetAuthor(int id)
        {
            return await _context.Authors
                .Include(a => a.Country)
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.AuthorId == id && !a.IsDeleted);
        }

        public async Task<List<Author>> GetAuthors()
        {
            return await _context.Authors.Include("Country").Where(a => !a.IsDeleted).ToListAsync();
        }
    }
}
