using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Library.Queries.GetBooksInLibraryQuery
{
    public class GetBooksLibraryInQueryDto
    {
        public List<LibraryBook> LibraryBooks { get; set; } = new List<LibraryBook>();
    }

    public class LibraryBook
    {
        public int BookId { get; set; }
        public string ISBN { get; set; }
        public string eISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
    }
}
