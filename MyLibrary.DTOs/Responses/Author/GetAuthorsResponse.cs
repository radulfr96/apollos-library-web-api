using MyLibrary.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.Common.Responses
{
    public class GetAuthorsResponse : BaseResponse
    {
        public List<AuthorListItemDTO> Authors { get; set; }

        public GetAuthorsResponse()
        {
            Authors = new List<AuthorListItemDTO>();
        }
    }
}
