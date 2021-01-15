using System;
using System.Collections.Generic;

namespace MyLibrary.Persistence.Model
{
    public partial class BookGenre
    {
        public int GenreId { get; set; }
        public int BookId { get; set; }

        public virtual Book Book { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
