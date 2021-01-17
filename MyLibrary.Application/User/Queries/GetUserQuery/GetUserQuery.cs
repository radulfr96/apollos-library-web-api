using MediatR;
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
    }

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserQueryDto>
    {
        public Task<GetUserQueryDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
