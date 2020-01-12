using MyLibrary.Common.DTOs;
using System;
using System.Collections.Generic;

namespace MyLibrary.Common.Responses
{
    public class GetUsersResponse : BaseResponse
    {
        public List<UserDTO> Users { get; set; }

        public GetUsersResponse()
        {
            Users = new List<UserDTO>();
        }
    }
}
