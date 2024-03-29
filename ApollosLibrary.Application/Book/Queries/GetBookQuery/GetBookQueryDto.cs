﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Book.Queries.GetBookQuery
{
    public class GetBookQueryDto
    {
        public int BookId { get; set; }
        public int BookRecordId { get; set; }
        public string ISBN { get; set; }
        public string EISBN { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public decimal? NumberInSeries { get; set; }
        public int? Edition { get; set; }
        public int PublicationFormatId { get; set; }
        public int FictionTypeId { get; set; }
        public int FormTypeId { get; set; }
        public int? BusinessId { get; set; }
        public string CoverImage { get; set; }
        public Guid CreatedBy { get; set; }
        public List<int> Genres { get; set; } = new List<int>();
        public List<int> Authors { get; set; } = new List<int>();
        public List<int> Series { get; set; } = new List<int>();
    }
}
