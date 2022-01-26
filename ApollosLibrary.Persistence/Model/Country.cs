using System;
using System.Collections.Generic;

#nullable disable

namespace ApollosLibrary.Persistence.Model
{
    public partial class Country
    {
        public Country()
        {
            Authors = new HashSet<Author>();
            Publishers = new HashSet<Publisher>();
        }

        public string CountryId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Author> Authors { get; set; }
        public virtual ICollection<Publisher> Publishers { get; set; }
    }
}
