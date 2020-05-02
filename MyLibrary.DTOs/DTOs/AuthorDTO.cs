using System;
using System.Collections.Generic;
using System.Text;

namespace MyLibrary.Common.DTOs
{
    public class AuthorDTO
    {
        public int AuthorID { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string CountryID { get; set; }
        public string Description { get; set; }
        public bool IsDeletable { get; set; }
    }
}
