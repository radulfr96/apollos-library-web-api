using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Moderation.Queries.GetEntryReportsByEntryUserQuery
{
    public class GetEntryReportsByEntryUserQueryDto
    {
        public List<EntryReportListItem> EntryReports { get; set; } = new List<EntryReportListItem>();
    }
}
