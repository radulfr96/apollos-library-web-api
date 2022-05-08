using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Library.Queries.GetLibraryEntryQuery
{
    public class GetLibraryEntryQueryDto
    {
        public int EntryId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
    }
}
