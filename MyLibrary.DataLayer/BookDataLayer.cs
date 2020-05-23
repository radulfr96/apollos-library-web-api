using MyLibrary.Data.Model;
using MyLibrary.DataLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLibrary.DataLayer
{
    public class BookDataLayer : IBookDataLayer
    {
        private MyLibraryContext _context;

        public BookDataLayer(MyLibraryContext context)
        {
            _context = context;
        }

        public Book GetBook(int id)
        {
            return _context.Book.FirstOrDefault(b => b.BookId == id);
        }
    }
}
