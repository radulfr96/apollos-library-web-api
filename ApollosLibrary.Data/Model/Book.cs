using System;
using System.Collections.Generic;

namespace MyLibrary.Data.Model
{
    public partial class Book
    {
        public Book()
        {
            BookGenre = new HashSet<BookGenre>();
        }

        public string Isbn { get; set; }
        public string Name { get; set; }

        public virtual ICollection<BookGenre> BookGenre { get; set; }
    }
}
