using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Domain
{
    #nullable disable

    public class BookRecord
    {
        public int BookRecordId { get; set; }
        public bool ReportedVersion { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
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
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
