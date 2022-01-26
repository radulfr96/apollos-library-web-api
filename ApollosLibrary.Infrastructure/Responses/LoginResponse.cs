using System;
using System.Collections.Generic;
using System.Text;

namespace ApollosLibrary.Infrastructure.Responses
{
    /// <summary>
    /// Used to return the users token if login is successful
    /// </summary>
    public class LoginResponse : BaseResponse
    {
        public string Token { get; set; }
    }
}
