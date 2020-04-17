using MyLibrary.Data.Model;
using MyLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLibrary.DataLayer
{
    public class AuthorDataLayer : IAuthorDataLayer
    {
        private MyLibraryContext _context;

        public AuthorDataLayer(MyLibraryContext context)
        {
            _context = context;
        }

        public void AddAuthor(Author author)
        {
            _context.Author.Add(author);
        }

        public void DeleteAuthor(int id)
        {
            var author = GetAuthor(id);

            _context.Author.Remove(author);
        }

        public Author GetAuthor(int id)
        {
            return _context.Author.FirstOrDefault(a => a.AuthorId == id);
        }

        public List<Author> GetAuthors()
        {
            return _context.Author.ToList();
        }
    }
}
