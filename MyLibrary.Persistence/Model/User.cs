using System;
using System.Collections.Generic;

#nullable disable

namespace MyLibrary.Persistence.Model
{
    public partial class User
    {
        public User()
        {
            BookCreatedByNavigations = new HashSet<Book>();
            BookModifiedByNavigations = new HashSet<Book>();
            GenreCreatedByNavigations = new HashSet<Genre>();
            GenreModifiedByNavigations = new HashSet<Genre>();
            PublisherCreatedByNavigations = new HashSet<Publisher>();
            PublisherModifiedByNavigations = new HashSet<Publisher>();
            UserRoles = new HashSet<UserRole>();
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

        public virtual ICollection<Book> BookCreatedByNavigations { get; set; }
        public virtual ICollection<Book> BookModifiedByNavigations { get; set; }
        public virtual ICollection<Genre> GenreCreatedByNavigations { get; set; }
        public virtual ICollection<Genre> GenreModifiedByNavigations { get; set; }
        public virtual ICollection<Publisher> PublisherCreatedByNavigations { get; set; }
        public virtual ICollection<Publisher> PublisherModifiedByNavigations { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
