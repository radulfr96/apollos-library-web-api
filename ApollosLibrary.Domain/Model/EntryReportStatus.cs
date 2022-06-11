using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Domain
{
    # nullable disable
    public class EntryReportStatus
    {
        [Key]
        public int EntryReportStatusId { get; set; }
        public string Name { get; set; }
    }
}
