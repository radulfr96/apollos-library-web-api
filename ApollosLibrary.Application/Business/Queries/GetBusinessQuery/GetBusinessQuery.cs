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

            var business = await _BusinessUnitOfWork.BusinessDataLayer.GetBusiness(query.BusinessId);

            if (business == null)
            {
                throw new BusinessNotFoundException($"Unable to find Business with id [{query.BusinessId}]");
            }

            response.BusinessRecordId = business.VersionId;
            response.City = business.City;
            response.CountryID = business.CountryId;
            response.Name = business.Name;
            response.Postcode = business.Postcode;
            response.BusinessId = business.BusinessId;
            response.State = business.State;
            response.StreetAddress = business.StreetAddress;
            response.Website = business.Website;
            response.CreatedBy = business.CreatedBy;

            return response;
        }
    }
}
