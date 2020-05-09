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

        public int BookId { get; set; }
        public string Isbn { get; set; }
        public string EIsbn { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public int? SeriesId { get; set; }
        public int? NumberInSeries { get; set; }
        public int? Edition { get; set; }
        public int PublicationFormatId { get; set; }
        public int FictionTypeId { get; set; }
        public int FormTypeId { get; set; }
        public string CoverImage { get; set; }

        public virtual FictionType FictionType { get; set; }
        public virtual FormType FormType { get; set; }
        public virtual PublicationFormat PublicationFormat { get; set; }
        public virtual ICollection<BookGenre> BookGenre { get; set; }
    }
}
