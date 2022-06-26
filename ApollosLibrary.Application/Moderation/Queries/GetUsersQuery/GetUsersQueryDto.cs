using ApollosLibrary.Domain.DTOs;
using System;
using System.Collections.Generic;

namespace ApollosLibrary.Application.Moderation.Queries.GetUsersQuery
{
    public class GetUsersQueryDto
    {
        public List<UserDTO> Users { get; set; } = new List<UserDTO>();
    }
}
