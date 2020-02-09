using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace MyLibrary.Common.Requests
{
    public class RegisterUserRequest : BaseRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a username")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a password")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide your confirmation password")]
        public string ConfirmPassword { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (!Regex.IsMatch(Password, "^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$"))
            {
                results.Add(new ValidationResult("Password is not strong enough"));
            }

            if (Password != ConfirmPassword)
            {
                results.Add(new ValidationResult("Password do not match"));
            }

            return results;
        }
    }
}
