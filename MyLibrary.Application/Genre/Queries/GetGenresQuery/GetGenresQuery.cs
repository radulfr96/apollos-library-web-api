using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Genre.Queries.GetGenresQuery
{
    public class GetGenresQuery : IRequest<GetGenresQueryDto>
    {
    }

    public class GetGenresQueryHandler : IRequestHandler<GetGenresQuery, GetGenresQueryDto>
    {
        public Task<GetGenresQueryDto> Handle(GetGenresQuery query, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
