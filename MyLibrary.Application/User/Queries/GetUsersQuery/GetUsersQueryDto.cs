using MyLibrary.Application.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Application.User.Queries.GetUsersQuery
{
    public class GetUsersQueryDto
    {
        public List<UserDTO> Users { get; set; } = new List<UserDTO>();
    }
}
