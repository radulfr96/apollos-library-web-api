using NodaTime;

namespace ApollosLibrary.Domain
{
    # nullable disable
    public class EntryReport
    {
        public int EntryReportId { get; set; }
        public int EntryRecordId { get; set; }
        public int EntryTypeId { get; set; }
        public EntryReportType EntryType { get; set; }

        public int EntryReportStatusId { get; set; }
        public EntryReportStatus EntryReportStatus { get; set; }
        
        public Guid ReportedBy { get; set; }
        public LocalDateTime ReportedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public LocalDateTime CreatedDate { get; set; }
    }
}
