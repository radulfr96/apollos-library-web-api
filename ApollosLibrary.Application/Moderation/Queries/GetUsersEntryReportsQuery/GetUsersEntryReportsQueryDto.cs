using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Moderation.Queries.GetUsersEntryReportsQuery
{
    public class GetUsersEntryReportsQueryDto
    {
        public List<EntryReportListItem> EntryReports { get; set; } = new List<EntryReportListItem>();
    }
}
