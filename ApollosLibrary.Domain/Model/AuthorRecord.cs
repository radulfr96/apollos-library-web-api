using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Domain
{
    #nullable disable

    public class AuthorRecord
    {
        public int AuthorRecordId { get; set; }
        public bool ReportedVersion { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }

        public string CountryId { get; set; }
        public Country Country { get; set; }

        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
