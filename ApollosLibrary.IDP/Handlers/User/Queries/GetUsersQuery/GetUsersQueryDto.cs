using ApollosLibrary.IDP.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.IDP.User.Queries.GetUsersQuery
{
    public class GetUsersQueryDto
    {
        public List<UserDTO> Users { get; set; } = new List<UserDTO>();
    }
}
