using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.Common.DTOs
{
    public class AuthorDTO
    {
        public int AuthorID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CountryID { get; set; }
        public string Description { get; set; }
    }
}
