using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Business.Queries.GetBusinesssQuery
{
    public class GetBusinesssQueryDto
    {
        public List<BusinessListItemDTO> Businesss { get; set; } = new List<BusinessListItemDTO>();
    }

    public class BusinessListItemDTO
    {
        public int BusinessId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
    }
}
