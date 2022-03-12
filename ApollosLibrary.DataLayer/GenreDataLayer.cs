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
    public class GenreDataLayer : IGenreDataLayer
    {
        public ApollosLibraryContext _context;

        public GenreDataLayer(ApollosLibraryContext context)
        {
            _context = context;
        }

        public async Task AddGenre(Genre genre)
        {
            await _context.AddAsync(genre);
        }

        public async Task DeleteGenre(int id)
        {
            _context.Genres.Remove(await _context.Genres.FirstOrDefaultAsync(g => g.GenreId == id));
        }

        public async Task<Genre> GetGenre(int id)
        {
            return await _context.Genres
                                .Include(g => g.Books)
                                .FirstOrDefaultAsync(g => g.GenreId == id);
        }

        public async Task<List<Genre>> GetGenres()
        {
            return await _context.Genres.ToListAsync();
        }
    }
}
