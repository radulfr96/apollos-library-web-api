using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Domain
{
    # nullable disable
    public class LibraryEntry
    {
        [Key]
        public int EntryId { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int Quantity { get; set; }
        public int LibraryId { get; set; }
        public Library Library { get; set; }
    }
}
