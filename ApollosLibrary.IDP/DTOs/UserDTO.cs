using System;
using System.Collections.Generic;

namespace ApollosLibrary.IDP.DTOs
{
    /// <summary>
    /// Data transfer object used to transfer users
    /// </summary>
    public class UserDTO
    {
        public Guid UserID { get; set; }
        public string Username { get; set; }
        public string IsActive { get; set; }

        public UserDTO()
        {
        }
    }
}
