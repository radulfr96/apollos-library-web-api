using System;
using System.Collections.Generic;

namespace MyLibrary.Persistence.Model
{
    public partial class Genre
    {
        public Genre()
        {
            BookGenre = new HashSet<BookGenre>();
        }

        public int GenreId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual User ModifiedByNavigation { get; set; }
        public virtual ICollection<BookGenre> BookGenre { get; set; }
    }
}
