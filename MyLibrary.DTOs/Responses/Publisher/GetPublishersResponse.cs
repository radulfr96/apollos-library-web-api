using MyLibrary.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.Common.Responses
{
    public class GetPublishersResponse : BaseResponse
    {
        public List<PublisherDTO> Publishers { get; set; }

        public GetPublishersResponse()
        {
            Publishers = new List<PublisherDTO>();
        }
    }
}
