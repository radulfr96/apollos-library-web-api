using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Library.Queries.GetLibraryEntriesQuery
{
    public class GetLibraryEntriesQueryDto
    {
        public List<LibraryEntryListItemDTO> LibraryEntries { get; set; } = new List<LibraryEntryListItemDTO>();
    }

    public class LibraryEntryListItemDTO
    {
        public int EntryId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Quantity { get; set; }
    }
}
