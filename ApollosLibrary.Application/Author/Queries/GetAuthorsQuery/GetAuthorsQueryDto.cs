using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Author.Queries.GetAuthorsQuery
{
    public class GetAuthorsQueryDto
    {
        public List<AuthorListItemDTO> Authors { get; set; } = new List<AuthorListItemDTO>();
    }

    public class AuthorListItemDTO
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
    }
}
