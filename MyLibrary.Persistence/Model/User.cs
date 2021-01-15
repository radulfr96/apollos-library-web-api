using System;
using System.Collections.Generic;

namespace MyLibrary.Persistence.Model
{
    public partial class User
    {
        public User()
        {
            BookCreatedByNavigation = new HashSet<Book>();
            BookModifiedByNavigation = new HashSet<Book>();
            GenreCreatedByNavigation = new HashSet<Genre>();
            GenreModifiedByNavigation = new HashSet<Genre>();
            PublisherCreatedByNavigation = new HashSet<Publisher>();
            PublisherModifiedByNavigation = new HashSet<Publisher>();
            UserRole = new HashSet<UserRole>();
        }

        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salter { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }

        public virtual ICollection<Book> BookCreatedByNavigation { get; set; }
        public virtual ICollection<Book> BookModifiedByNavigation { get; set; }
        public virtual ICollection<Genre> GenreCreatedByNavigation { get; set; }
        public virtual ICollection<Genre> GenreModifiedByNavigation { get; set; }
        public virtual ICollection<Publisher> PublisherCreatedByNavigation { get; set; }
        public virtual ICollection<Publisher> PublisherModifiedByNavigation { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
