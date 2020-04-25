using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyLibrary.Common.Requests
{
    public class AddAuthorRequest : BaseRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must supply a first name or an alias")]
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must select a country of origin")]
        public string CountryID { get; set; }
        public string Description { get; set; }
    }
}
