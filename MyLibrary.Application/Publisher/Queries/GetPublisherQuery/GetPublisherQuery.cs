using MediatR;
using MyLibrary.UnitOfWork.Contracts;
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
        public int PublisherId { get; set; }
    }

    public class GetPublicQueryHandler : IRequestHandler<GetPublisherQuery, GetPublisherQueryDto>
    {
        private readonly IPublisherUnitOfWork _publisherUnitOfWork;

        public GetPublicQueryHandler(IPublisherUnitOfWork publisherUnitOfWork)
        {
            _publisherUnitOfWork = publisherUnitOfWork;
        }

        public async Task<GetPublisherQueryDto> Handle(GetPublisherQuery query, CancellationToken cancellationToken)
        {
            var response = new GetPublisherQueryDto();

            var publisher = await _publisherUnitOfWork.PublisherDataLayer.GetPublisher(query.PublisherId);

            if (publisher == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }

            response.City = publisher.City;
            response.CountryID = publisher.CountryId;
            response.Name = publisher.Name;
            response.Postcode = publisher.Postcode;
            response.PublisherId = publisher.PublisherId;
            response.State = publisher.State;
            response.StreetAddress = publisher.StreetAddress;
            response.Website = publisher.Website;

            return response;
        }
    }
}
