using NodaTime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

# nullable disable

namespace ApollosLibrary.Domain
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        public int VersionId { get; set; }
        public string Isbn { get; set; }
        public string EIsbn { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public int? Edition { get; set; }
        public bool IsDeleted { get; set; }

        public int PublicationFormatId { get; set; }
        public PublicationFormat PublicationFormat { get; set; }

        public int FictionTypeId { get; set; }
        public FictionType FictionType { get; set; }

        public int FormTypeId { get; set; }
        public FormType FormType { get; set; }

        public int? BusinessId { get; set; }
        public Business Business { get; set; }
        public string CoverImage { get; set; }
        public LocalDateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public ICollection<Author> Authors { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public ICollection<Series> Series { get; set; }
        public ICollection<BookRecord> BookRecords { get; set; } = new List<BookRecord>();
    }
}
