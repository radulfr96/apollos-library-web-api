using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Domain
{
    #nullable disable
    public class BusinessRecord
    {
        public int BusinessRecordId { get; set; }
        public bool ReportedVersion { get; set; }
        public int BusinessId { get; set; }
        public Business Business { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string State { get; set; }

        public string CountryId { get; set; }
        public Country Country { get; set; }

        public int BusinessTypeId { get; set; }
        public BusinessType Type { get; set; }

        public bool? IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
