using System;
using System.Collections.Generic;

namespace MyLibrary.Data.Model
{
    public partial class Book
    {
        public Book()
        {
            BookAuthor = new HashSet<BookAuthor>();
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
        public int PublisherId { get; set; }
        public string CoverImage { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual FictionType FictionType { get; set; }
        public virtual FormType FormType { get; set; }
        public virtual User ModifiedByNavigation { get; set; }
        public virtual PublicationFormat PublicationFormat { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual ICollection<BookAuthor> BookAuthor { get; set; }
        public virtual ICollection<BookGenre> BookGenre { get; set; }
    }
}
