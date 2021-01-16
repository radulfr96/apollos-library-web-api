using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Application.Author.Queries.GetAuthorQuery
{
    public class GetAuthorQueryDto
    {
        public int AuthorID { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string CountryID { get; set; }
        public string Description { get; set; }
        public bool IsDeletable { get; set; }
    }
}
