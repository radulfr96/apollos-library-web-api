using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Moderation.Queries
{
    public class EntryReportListItem
    {
        public int ReportId { get; set; }
        public int EntryId { get; set; }
        public int EntryTypeId { get; set; }
        public string EntryType { get; set; }
        public int EntryStatusId { get; set; }
        public string EntryStatus { get; set; }
        public Guid ReportedBy { get; set; }
        public DateTime ReportedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
