using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Business.Queries.GetBusinessQuery
{
    public class GetBusinessQueryDto
    {
        public int BusinessId { get; set; }
        public int BusinessRecordId { get; set; }
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string State { get; set; }
        public string CountryID { get; set; }
        public string Website { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
