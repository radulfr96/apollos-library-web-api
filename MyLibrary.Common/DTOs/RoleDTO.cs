using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.Common.DTOs
{
    /// <summary>
    /// Used as a data transfer object for user roles
    /// </summary>
    public class RoleDTO
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
    }
}
