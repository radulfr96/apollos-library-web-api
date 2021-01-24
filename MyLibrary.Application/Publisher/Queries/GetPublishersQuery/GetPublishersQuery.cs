using MediatR;
using MyLibrary.UnitOfWork.Contracts;
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
        private readonly IPublisherUnitOfWork _publisherUnitOfWork;

        public GetPublisherQueryHandler(IPublisherUnitOfWork publisherUnitOfWork)
        {
            _publisherUnitOfWork = publisherUnitOfWork;
        }

        public async Task<GetPublishersQueryDto> Handle(GetPublishersQuery request, CancellationToken cancellationToken)
        {
            var response = new GetPublishersQueryDto();

            var publishers = await _publisherUnitOfWork.PublisherDataLayer.GetPublishers();

            if (publishers.Count == 0)
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }

            response.Publishers = publishers.Select(p => new PublisherListItemDTO()
            {
                Country = p.Country.Name,
                Name = p.Name,
                PublisherId = p.PublisherId
            }).ToList();

            return response;
        }
    }
}
