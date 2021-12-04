using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Application.Genre.Queries.GetGenresQuery
{
    public class GetGenresQueryDto
    {
        public List<GenreDto> Genres { get; set; } = new List<GenreDto>();
    }

    public class GenreDto
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
    }
}
