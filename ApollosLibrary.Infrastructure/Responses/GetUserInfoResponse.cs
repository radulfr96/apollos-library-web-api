using System;
using System.Collections.Generic;
using System.Text;

namespace ApollosLibrary.Infrastructure.Responses
{
    /// <summary>
    /// Used to return information about a specific user
    /// </summary>
    public class GetUserInfoResponse : BaseResponse
    {
        public string Username { get; set; }
        public List<string> Roles { get; set; }
        public int UserID { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
