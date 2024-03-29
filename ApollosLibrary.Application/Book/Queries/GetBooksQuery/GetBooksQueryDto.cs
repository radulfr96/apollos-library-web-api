﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Book.Queries.GetBooksQuery
{
    public class GetBooksQueryDto
    {
        public List<BookListItemDTO> Books { get; set; } = new List<BookListItemDTO>();
    }

    public class BookListItemDTO
    {
        public int BookId { get; set; }
        public string ISBN { get; set; }
        public string eISBN { get; set; }
        public string Title { get; set; }
        public string FormatType { get; set; }
        public string FictionType { get; set; }
    }
}
