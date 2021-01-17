using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Publisher.Queries.GetPublisherQuery
{
    public class GetPublisherQuery : IRequest<GetPublisherQueryDto>
    {
    }

    public class GetPublicQueryHandler : IRequestHandler<GetPublisherQuery, GetPublisherQueryDto>
    {
        public Task<GetPublisherQueryDto> Handle(GetPublisherQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
