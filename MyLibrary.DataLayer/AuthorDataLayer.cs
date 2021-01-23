using Microsoft.EntityFrameworkCore;
using MyLibrary.DataLayer.Contracts;
using MyLibrary.Persistence.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.DataLayer
{
    public class AuthorDataLayer : IAuthorDataLayer
    {
        private MyLibraryContext _context;

        public AuthorDataLayer(MyLibraryContext context)
        {
            _context = context;
        }

        public async Task AddAuthor(Author author)
        {
            await _context.Author.AddAsync(author);
        }

        public async Task DeleteAuthor(int id)
        {
            var author = await GetAuthor(id);

            _context.Author.Remove(author);
        }

        public async Task<Author> GetAuthor(int id)
        {
            return await _context.Author.Include("Country").FirstOrDefaultAsync(a => a.AuthorId == id);
        }

        public async Task<List<Author>> GetAuthors()
        {
            return await _context.Author.Include("Country").ToListAsync();
        }
    }
}
