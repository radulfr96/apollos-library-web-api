using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Series.Queries.GetMultiSeriesQuery
{
    public class GetMultiSeriesQueryDto
    {
        public List<SeriesListItemDTO> Series { get; set; } = new List<SeriesListItemDTO>();
    }

    public class SeriesListItemDTO
    {
        public int SeriesId { get; set; }
        public string Name { get; set; }
    }
}
