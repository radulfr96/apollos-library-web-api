using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Publisher.Queries.GetPublishersQuery
{
    public class GetPublishersQuery : IRequest<GetPublishersQueryDto> 
    {
    }

    public class GetPublisherQueryHandler : IRequestHandler<GetPublishersQuery, GetPublishersQueryDto>
    {
        public Task<GetPublishersQueryDto> Handle(GetPublishersQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
