using System;
using System.Collections.Generic;

namespace MyLibrary.Data.Model
{
    public partial class BookGenre
    {
        public int GenreId { get; set; }
        public string Isbn { get; set; }

        public virtual Genre Genre { get; set; }
        public virtual Book IsbnNavigation { get; set; }
    }
}
