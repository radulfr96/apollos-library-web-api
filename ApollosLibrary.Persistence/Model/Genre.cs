using System;
using System.Collections.Generic;

#nullable disable

namespace ApollosLibrary.Persistence.Model
{
    public partial class Genre
    {
        public Genre()
        {
            BookGenres = new HashSet<BookGenre>();
        }

        public int GenreId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }

        public virtual ICollection<BookGenre> BookGenres { get; set; }
    }
}
