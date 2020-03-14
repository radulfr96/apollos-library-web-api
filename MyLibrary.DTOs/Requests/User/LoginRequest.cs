using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyLibrary.Common.Requests
{
    /// <summary>
    /// Request used to login a user
    /// </summary>
    public class LoginRequest : BaseRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide your username to login.")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide your password to login.")]
        public string Password { get; set; }
    }
}
