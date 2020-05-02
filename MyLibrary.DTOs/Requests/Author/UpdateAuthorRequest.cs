using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyLibrary.Common.Requests
{
    public class UpdateAuthorRequest : BaseRequest
    {
        public int AuthorID { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must supply a first name or an alias")]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must select a country of origin")]
        public string CountryID { get; set; }
        public string Description { get; set; }
    }
}
