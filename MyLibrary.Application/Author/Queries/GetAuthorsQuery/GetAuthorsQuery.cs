using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Author.Queries.GetAuthorsQuery
{
    public class GetAuthorsQuery : IRequest<GetAuthorsQueryDto>
    {
    }

    public class GetAuthorsQueryHandler : IRequestHandler<GetAuthorsQuery, GetAuthorsQueryDto>
    {
        public Task<GetAuthorsQueryDto> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
