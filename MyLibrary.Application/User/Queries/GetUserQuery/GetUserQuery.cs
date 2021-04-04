using MediatR;
using MyLibrary.Application.Common.DTOs;
using MyLibrary.Application.Common.Exceptions;
using MyLibrary.Contracts.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.User.Queries.GetUserQuery
{
    public class GetUserQuery : IRequest<GetUserQueryDto>
    {
        public Guid UserId { get; set; }
    }

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserQueryDto>
    {
        private readonly IUserUnitOfWork _userUnitOfWork;

        public GetUserQueryHandler(IUserUnitOfWork userUnitOfWork)
        {
            _userUnitOfWork = userUnitOfWork;
        }

        public async Task<GetUserQueryDto> Handle(GetUserQuery query, CancellationToken cancellationToken)
        {
            var response = new GetUserQueryDto();

            //var user = await _userUnitOfWork.UserDataLayer.GetUser(query.UserId);

            //if (user == null)
            //{
            //    throw new UserNotFoundException($"Unable to finc user with id [{query.UserId}]");
            //}

            //response.IsActive = user.IsActive ? "Active" : "Inactive";
            //response.UserID = user.UserId;
            //response.Username = user.Username;

            return response;
        }
    }
}
