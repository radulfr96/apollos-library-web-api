using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.IDP.User.Queries.GetUserQuery
{
    public class GetUserQueryDto
    {
        public Guid UserID { get; set; }
        public string Username { get; set; }
        public string IsActive { get; set; }
        public List<string> UserRoles { get; set; } = new List<string>();
        public List<string> Roles { get; set; } = new List<string>();
    }
}
