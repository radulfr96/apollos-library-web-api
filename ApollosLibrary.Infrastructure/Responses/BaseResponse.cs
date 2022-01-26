using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ApollosLibrary.Infrastructure.Responses
{
    /// <summary>
    /// Used as a base for all reponses so that they all have status codes and status messages
    /// </summary>
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
