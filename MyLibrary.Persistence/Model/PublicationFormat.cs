﻿using System;
using System.Collections.Generic;

namespace MyLibrary.Persistence.Model
{
    public partial class PublicationFormat
    {
        public PublicationFormat()
        {
            Book = new HashSet<Book>();
        }

        public int TypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Book> Book { get; set; }
    }
}
