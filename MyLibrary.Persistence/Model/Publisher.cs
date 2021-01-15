using System;
using System.Collections.Generic;

namespace MyLibrary.Persistence.Model
{
    public partial class Publisher
    {
        public Publisher()
        {
            Book = new HashSet<Book>();
        }

        public int PublisherId { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string State { get; set; }
        public string CountryId { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }

        public virtual Country Country { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User ModifiedByNavigation { get; set; }
        public virtual ICollection<Book> Book { get; set; }
    }
}
