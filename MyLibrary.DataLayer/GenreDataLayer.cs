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
    public class GenreDataLayer : IGenreDataLayer
    {
        public MyLibraryContext _context;

        public GenreDataLayer(MyLibraryContext context)
        {
            _context = context;
        }

        public async Task AddGenre(Genre genre)
        {
            await _context.AddAsync(genre);
        }

        public async Task DeleteGenre(int id)
        {
            var bookGenres = await _context.BookGenres.Where(bg => bg.GenreId == id).ToListAsync();

            _context.BookGenres.RemoveRange(bookGenres);

            _context.Genres.Remove(await _context.Genres.FirstOrDefaultAsync(g => g.GenreId == id));
        }

        public async Task<Genre> GetGenre(int id)
        {
            return await _context.Genres.FirstOrDefaultAsync(g => g.GenreId == id);
        }

        public async Task<List<Genre>> GetGenres()
        {
            return await _context.Genres.ToListAsync();
        }
    }
}
