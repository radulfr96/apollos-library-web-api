using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

# nullable disable

namespace ApollosLibrary.Domain
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
        public LocalDateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public LocalDateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
