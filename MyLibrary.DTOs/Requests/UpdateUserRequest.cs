using MyLibrary.Common.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace MyLibrary.Common.Requests
{
    public class UpdateUserRequest : BaseRequest
    {
        public int UserID { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "User must have a username")]
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmationPassword { get; set; }
        [MinLength(1, ErrorMessage = "User must have a role")]
        public List<RoleDTO> Roles { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (!string.IsNullOrEmpty(Password))
            {
                if (!Regex.IsMatch(Password, "^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$"))
                {
                    results.Add(new ValidationResult("Password is not strong enough"));
                }

                if (Password != ConfirmationPassword)
                {
                    results.Add(new ValidationResult("Password do not match"));
                }
            }

            return results;
        }
    }
}
