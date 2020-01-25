using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.Common.Responses
{
    public class LoginResponse : BaseResponse
    {
        public string Token { get; set; }
    }
}
