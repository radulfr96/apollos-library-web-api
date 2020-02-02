using MyLibrary.Common.DTOs;
using System;
using System.Collections.Generic;

namespace MyLibrary.Common.Responses
{
    /// <summary>
    /// Used to return a list of all of the users
    /// </summary>
    public class GetUsersResponse : BaseResponse
    {
        public List<UserDTO> Users { get; set; }

        public GetUsersResponse()
        {
            Users = new List<UserDTO>();
        }
    }
}
