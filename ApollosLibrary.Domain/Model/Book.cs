using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Domain
{
    public class Book
    {
        public int BookId { get; set; }
        public string Isbn { get; set; }
        public string EIsbn { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public int? SeriesId { get; set; }
        public decimal? NumberInSeries { get; set; }
        public int? Edition { get; set; }
        public PublicationFormat PublicationFormat { get; set; }
        public FictionType FictionType { get; set; }
        public FormType FormType { get; set; }
        public Publisher Publisher { get; set; }
        public string CoverImage { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public Series Series { get; set; }
        public ICollection<Author> Authors { get; set; }
    }
}
