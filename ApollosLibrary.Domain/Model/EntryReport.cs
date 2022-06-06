using ApollosLibrary.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Domain
{
    public class EntryReport
    {
        public int EntryReportId { get; set; }
        public int EntryId { get; set; }
        public EntryTypeEnum EntryType { get; set; }
        public Guid ReportedBy { get; set; }
        public DateTime ReportedDate { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
