using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyLibrary.Common.Requests
{
    public class UpdateUsernameRequest : BaseRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must provide a new username")]
        public string NewUsername { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must provide your password to update your username")]
        public string Password { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            return results;
        }
    }
}
