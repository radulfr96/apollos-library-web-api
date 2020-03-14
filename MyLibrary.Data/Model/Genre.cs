using System;
using System.Collections.Generic;

namespace MyLibrary.Data.Model
{
    public partial class Genre
    {
        public Genre()
        {
            BookGenre = new HashSet<BookGenre>();
        }

        public int GenreId { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public virtual ICollection<BookGenre> BookGenre { get; set; }
    }
}
