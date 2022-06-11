using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Domain
{
    # nullable disable
    public class EntryReportType
    {
        [Key]
        public int EntryReportTypeId { get; set; }
        public string Name { get; set; }
    }
}
