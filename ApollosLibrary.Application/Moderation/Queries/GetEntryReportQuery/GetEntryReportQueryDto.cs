﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApollosLibrary.Domain.Enums;
using NodaTime;

namespace ApollosLibrary.Application.Moderation.Queries.GetEntryReportQuery
{
    public class GetEntryReportQueryDto
    {
        public int EntryRecordId { get; set; }
        public EntryTypeEnum EntryTypeId { get; set; }
        public string EntryType { get; set; }
        public int EntryReportStatusId { get; set; }
        public string EntryStatus { get; set; }
        public Guid ReportedBy { get; set; }
        public LocalDateTime ReportedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public LocalDateTime CreatedDate { get; set; }
    }
}
