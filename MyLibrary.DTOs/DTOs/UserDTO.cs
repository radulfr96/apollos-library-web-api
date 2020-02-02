using System;
using System.Collections.Generic;

namespace MyLibrary.Common.DTOs
{
    public class UserDTO
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public bool SetPassword { get; set; }
        public string IsActive { get; set; }
        public List<RoleDTO> Roles { get; set; }

        public UserDTO()
        {
            Roles = new List<RoleDTO>();
        }
    }
}
