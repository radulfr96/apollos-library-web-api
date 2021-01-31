using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Application.Book.Queries.GetBookQuery
{
    public class GetBookQueryDto
    {
        public int BookID { get; set; }
        public string ISBN { get; set; }
        public string eISBN { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public int? SeriesID { get; set; }
        public decimal? NumberInSeries { get; set; }
        public int? Edition { get; set; }
        public int PublicationFormat { get; set; }
        public int FictionType { get; set; }
        public int FormType { get; set; }
        public int Publisher { get; set; }
        public byte[] CoverImage { get; set; }
        public List<int> Genres { get; set; }
        public List<int> Authors { get; set; }
    }
}
