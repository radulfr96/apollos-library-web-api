using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.Common.Responses
{
    public class GetUserInfoResponse : BaseResponse
    {
        public string Username { get; set; }
        public List<string> Roles { get; set; }
    }
}
