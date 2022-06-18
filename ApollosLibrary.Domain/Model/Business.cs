using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

# nullable disable

namespace ApollosLibrary.Domain
{
    public class Business
    {
        public int BusinessId { get; set; }
        public int VersionId { get; set; }
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
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public ICollection<BusinessRecord> BusinessRecords { get; set; }
    }
}
