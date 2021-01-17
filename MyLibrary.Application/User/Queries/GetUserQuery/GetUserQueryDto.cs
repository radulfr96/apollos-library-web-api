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
        public UserDTO User { get; set; } = new UserDTO();
        public List<RoleDTO> Roles { get; set; } = new List<RoleDTO>();
    }
}
