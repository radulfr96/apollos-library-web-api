using MediatR;
using MyLibrary.Application.Common.DTOs;
using MyLibrary.Contracts.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.User.Queries.GetUsersQuery
{
    public class GetUsersQuery : IRequest<GetUsersQueryDto>
    {
    }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, GetUsersQueryDto>
    {
        private readonly IUserUnitOfWork _userUnitOfWork;

        public GetUsersQueryHandler(IUserUnitOfWork userUnitOfWork)
        {
            _userUnitOfWork = userUnitOfWork;
        }

        public async Task<GetUsersQueryDto> Handle(GetUsersQuery query, CancellationToken cancellationToken)
        {
            var response = new GetUsersQueryDto();

            var users = await _userUnitOfWork.UserDataLayer.GetUsers();

            if (users.Count == 0)
            {
                return response;
            }

            response.Users = users.Select(u => new UserDTO()
            {
                UserID = u.UserId,
                IsActive = u.IsActive ? "Active" : "Inactive",
                Username = u.Username,
                Roles = u.UserRole.Select(r => new RoleDTO()
                {
                    Name = r.Role.Name,
                    RoleId = r.RoleId,
                }).ToList()
            }).ToList();

            return response;
        }
    }
}
