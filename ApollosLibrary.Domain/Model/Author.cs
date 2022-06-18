using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

# nullable disable

namespace ApollosLibrary.Domain
{
    public class Author
    {
        public int AuthorId { get; set; }
        public int VersionId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }

        public string CountryId { get; set; }
        public Country Country { get; set; }

        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public ICollection<Book> Books { get; set; }
        public ICollection<AuthorRecord> AuthorRecords { get; set; }
    }
}
