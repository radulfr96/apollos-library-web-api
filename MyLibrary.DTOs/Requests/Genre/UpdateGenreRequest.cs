using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyLibrary.Common.Requests
{
    public class UpdateGenreRequest : BaseRequest
    {
        public int GenreID { get; set; }
        [Required (AllowEmptyStrings = false, ErrorMessage = "You must provide a genre name")]
        public string Name { get; set; }
    }
}
