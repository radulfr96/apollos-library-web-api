using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.Common.Responses
{
    /// <summary>
    /// Used to return information about a specific user
    /// </summary>
    public class GetUserInfoResponse : BaseResponse
    {
        public string Username { get; set; }
        public List<string> Roles { get; set; }
    }
}
