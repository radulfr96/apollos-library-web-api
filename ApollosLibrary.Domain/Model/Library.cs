using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Domain
{
    # nullable disable

    public class Library
    {
        [Key]
        public int LibraryId { get; set; }
        public Guid UserId { get; set; }
        public List<LibraryEntry> LibraryEntries { get; set; } = new List<LibraryEntry>();
    }
}
