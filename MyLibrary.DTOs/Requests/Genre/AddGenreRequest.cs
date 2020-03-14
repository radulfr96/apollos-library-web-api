using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyLibrary.Common.Requests
{
    public class AddGenreRequest : BaseRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must supply a genre name")]
        public string Name { get; set; }
    }
}
