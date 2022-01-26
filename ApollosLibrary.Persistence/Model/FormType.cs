using System;
using System.Collections.Generic;

#nullable disable

namespace ApollosLibrary.Persistence.Model
{
    public partial class FormType
    {
        public FormType()
        {
            Books = new HashSet<Book>();
        }

        public int TypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
