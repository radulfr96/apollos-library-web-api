using MyLibrary.Application.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Application.User.Queries.GetUserQuery
{
    public class GetUserQueryDto
    {
        public Guid UserID { get; set; }
        public string Username { get; set; }
        public string IsActive { get; set; }
    }
}
