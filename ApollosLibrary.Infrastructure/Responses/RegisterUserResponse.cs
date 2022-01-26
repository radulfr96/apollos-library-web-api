using System;
using System.Collections.Generic;
using System.Text;

namespace ApollosLibrary.Infrastructure.Responses
{
    public class RegisterUserResponse : BaseResponse
    {
        public string Token { get; set; }
    }
}
