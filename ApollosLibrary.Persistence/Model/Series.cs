using System;
using System.Collections.Generic;

#nullable disable

namespace ApollosLibrary.Persistence.Model
{
    public partial class Series
    {
        public Series()
        {
            Books = new HashSet<Book>();
        }

        public int SeriesId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
