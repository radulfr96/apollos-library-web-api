using MediatR;
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
        public Task<GetUsersQueryDto> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
