using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Book.Queries.GetBookQuery
{
    public class GetBookQuery : IRequest<GetBookQueryDto> { }

    public class GetBookQueryHandler : IRequestHandler<GetBookQuery, GetBookQueryDto>
    {
        public Task<GetBookQueryDto> Handle(GetBookQuery query, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
