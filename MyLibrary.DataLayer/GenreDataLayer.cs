using MyLibrary.Data.Model;
using MyLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLibrary.DataLayer
{
    public class GenreDataLayer : IGenreDataLayer
    {
        public MyLibraryContext _context;

        public GenreDataLayer(MyLibraryContext context)
        {
            _context = context;
        }

        public void AddGenre(Genre genre)
        {
            _context.Add(genre);
        }

        public void DeleteGenre(int id)
        {
            var bookGenres = _context.BookGenre.Where(bg => bg.GenreId == id).ToList();

            _context.BookGenre.RemoveRange(bookGenres);

            _context.Genre.Remove(_context.Genre.FirstOrDefault(g => g.GenreId == id));
        }

        public Genre GetGenre(int id)
        {
            return _context.Genre.FirstOrDefault(g => g.GenreId == id);
        }

        public List<Genre> GetGenres()
        {
            return _context.Genre.ToList();
        }
    }
}
