using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApollosLibrary.Domain.Enums;

namespace ApollosLibrary.Application.Moderation.Queries.GetEntryReportQuery
{
    public class GetEntryReportQueryDto
    {
        public int EntryId { get; set; }
        public EntryTypeEnum EntryTypeId { get; set; }
        public string EntryType { get; set; }
        public int EntryReportStatusId { get; set; }
        public string EntryStatus { get; set; }
        public Guid ReportedBy { get; set; }
        public DateTime ReportedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
