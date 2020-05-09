using System;
using System.Collections.Generic;

namespace MyLibrary.Common.DTOs
{
    /// <summary>
    /// Data transfer object used to transfer users
    /// </summary>
    public class UserDTO
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string IsActive { get; set; }
        public List<RoleDTO> Roles { get; set; }

        public UserDTO()
        {
            Roles = new List<RoleDTO>();
        }
    }
}
