using System;
using System.Collections.Generic;

namespace MyLibrary.Persistence.Model
{
    public partial class Author
    {
        public Author()
        {
            BookAuthor = new HashSet<BookAuthor>();
        }

        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public string CountryId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<BookAuthor> BookAuthor { get; set; }
    }
}
