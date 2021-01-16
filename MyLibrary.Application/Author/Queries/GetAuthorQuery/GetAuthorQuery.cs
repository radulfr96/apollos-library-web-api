using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Author.Queries.GetAuthorQuery
{
    public class GetAuthorQuery : IRequest<GetAuthorQueryDto>
    {
    }

    public class GetAuthorQueryHandler : IRequestHandler<GetAuthorQuery, GetAuthorQueryDto>
    {
        public Task<GetAuthorQueryDto> Handle(GetAuthorQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
