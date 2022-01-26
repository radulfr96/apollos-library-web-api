using ApollosLibrary.Application.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.User.Queries.GetUsersQuery
{
    public class GetUsersQueryDto
    {
        public List<UserDTO> Users { get; set; } = new List<UserDTO>();
    }
}
