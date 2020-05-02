using MyLibrary.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.Common.Responses
{
    public class GetPublisherResponse : BaseResponse
    {
        public PublisherDTO Publisher { get; set; }
    }
}
