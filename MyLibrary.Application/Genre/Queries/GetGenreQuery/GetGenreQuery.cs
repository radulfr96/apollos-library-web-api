using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Genre.Queries.GetGenreQuery
{
    public class GetGenreQuery : IRequest<GetGenreQueryDto>
    {
    }

    public class GetGenreQueryHandler : IRequestHandler<GetGenreQuery, GetGenreQueryDto>
    {
        public Task<GetGenreQueryDto> Handle(GetGenreQuery query, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
