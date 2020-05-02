using System;
using System.Collections.Generic;

namespace MyLibrary.Data.Model
{
    public partial class Country
    {
        public Country()
        {
            Author = new HashSet<Author>();
            Publisher = new HashSet<Publisher>();
        }

        public string CountryId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Author> Author { get; set; }
        public virtual ICollection<Publisher> Publisher { get; set; }
    }
}
