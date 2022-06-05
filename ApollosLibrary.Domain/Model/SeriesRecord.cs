using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Domain
{
    #nullable disable

    public class SeriesRecord
    {
        public int SeriesRecordId { get; set; }
        public bool ReportedVersion { get; set; }
        public int SeriesId { get; set; }
        public Series Series { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
