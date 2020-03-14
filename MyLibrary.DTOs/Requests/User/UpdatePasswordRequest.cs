using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace MyLibrary.Common.Requests
{
    public class UpdatePasswordRequest : BaseRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Current password required")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "New password required")]
        public string NewPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "New password confirmation required")]
        public string NewPasswordConfirmation { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (!Regex.IsMatch(NewPassword, "^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$"))
            {
                results.Add(new ValidationResult("New password is not strong enough"));
            }

            if (NewPassword != NewPasswordConfirmation)
            {
                results.Add(new ValidationResult("New password confirmation does not match"));
            }

            return results;
        }
    }
}
