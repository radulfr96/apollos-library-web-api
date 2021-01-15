using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Book.Queries
{
    public class GetBooksQuery : IRequest<GetBooksQueryDto>
    {
    }

    public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, GetBooksQueryDto>
    {
        public Task<GetBooksQueryDto> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
