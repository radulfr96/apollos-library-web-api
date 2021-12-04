using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Application.Publisher.Queries.GetPublishersQuery
{
    public class GetPublishersQueryDto
    {
        public List<PublisherListItemDTO> Publishers { get; set; } = new List<PublisherListItemDTO>();
    }

    public class PublisherListItemDTO
    {
        public int PublisherId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
    }
}
