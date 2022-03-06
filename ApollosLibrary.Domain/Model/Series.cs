using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

# nullable disable

namespace ApollosLibrary.Domain
{
    public class Series
    {
        public int SeriesId { get; set; }
        public string Name { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
