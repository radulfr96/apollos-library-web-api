using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Domain.DTOs
{
    #nullable disable
    
    public class UserDTO
    {
        public Guid UserID { get; set; }
        public int ReportsOfUser { get; set; }
        public int ReportsByUser { get; set; }
    }
}
