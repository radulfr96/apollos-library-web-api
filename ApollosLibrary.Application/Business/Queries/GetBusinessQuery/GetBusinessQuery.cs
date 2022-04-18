using MediatR;
using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Business.Queries.GetBusinessQuery
{
    public class GetBusinessQuery : IRequest<GetBusinessQueryDto>
    {
        public int BusinessId { get; set; }
    }

    public class GetPublicQueryHandler : IRequestHandler<GetBusinessQuery, GetBusinessQueryDto>
    {
        private readonly IBusinessUnitOfWork _BusinessUnitOfWork;

        public GetPublicQueryHandler(IBusinessUnitOfWork BusinessUnitOfWork)
        {
            _BusinessUnitOfWork = BusinessUnitOfWork;
        }

        public async Task<GetBusinessQueryDto> Handle(GetBusinessQuery query, CancellationToken cancellationToken)
        {
            var response = new GetBusinessQueryDto();

            var Business = await _BusinessUnitOfWork.BusinessDataLayer.GetBusiness(query.BusinessId);

            if (Business == null)
            {
                throw new BusinessNotFoundException($"Unable to find Business with id [{query.BusinessId}]");
            }

            response.City = Business.City;
            response.CountryID = Business.CountryId;
            response.Name = Business.Name;
            response.Postcode = Business.Postcode;
            response.BusinessId = Business.BusinessId;
            response.State = Business.State;
            response.StreetAddress = Business.StreetAddress;
            response.Website = Business.Website;

            return response;
        }
    }
}
