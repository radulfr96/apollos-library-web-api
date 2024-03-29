﻿using NodaTime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Domain
{
    #nullable disable

    public class BookRecord
    {
        [Key]
        public int BookRecordId { get; set; }
        public bool ReportedVersion { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public string Isbn { get; set; }
        public string EIsbn { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public int? Edition { get; set; }
        public bool IsDeleted { get; set; }
        public int PublicationFormatId { get; set; }
        public int FictionTypeId { get; set; }
        public int FormTypeId { get; set; }
        public int? BusinessId { get; set; }
        public string CoverImage { get; set; }
        public LocalDateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
