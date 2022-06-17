using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Book.Queries.GetBookRecordQuery
{
    public class GetBookRecordQueryDto
    {
        public int BookId { get; set; }
        public string ISBN { get; set; }
        public string EISBN { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string CoverImage { get; set; }
    }
}
