using MyLibrary.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.Common.Responses
{
    public class GetUserResponse : BaseResponse
    {
        public UserDTO User { get; set; }
        public List<RoleDTO> Roles { get; set; }
    }
}
