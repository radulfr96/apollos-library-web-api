using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MyLibrary.Common.Responses
{
    public class BaseResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public List<string> Messages { get; set; }

        public BaseResponse()
        {
            StatusCode = HttpStatusCode.InternalServerError;
            Messages = new List<string>();
        }
    }
}
