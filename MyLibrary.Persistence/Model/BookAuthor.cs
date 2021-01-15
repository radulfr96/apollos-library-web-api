using System;
using System.Collections.Generic;

namespace MyLibrary.Persistence.Model
{
    public partial class BookAuthor
    {
        public int AuthorId { get; set; }
        public int BookId { get; set; }

        public virtual Author Author { get; set; }
        public virtual Book Book { get; set; }
    }
}
