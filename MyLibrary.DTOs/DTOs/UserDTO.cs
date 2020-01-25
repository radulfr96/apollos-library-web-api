using System;

namespace MyLibrary.Common.DTOs
{
    public class UserDTO
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public bool SetPassword { get; set; }
        public string IsActive { get; set; }
    }
}
