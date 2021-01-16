using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Book.Queries.GetBooksQuery
{
    public class GetBooksQuery : IRequest<GetBooksQueryDto> { }

    public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, GetBooksQueryDto>
    {
        public Task<GetBooksQueryDto> Handle(GetBooksQuery query, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
