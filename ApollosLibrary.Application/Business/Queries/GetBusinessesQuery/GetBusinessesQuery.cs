using MediatR;
using ApollosLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ApollosLibrary.Domain.Enums;

namespace ApollosLibrary.Application.Business.Queries.GetBusinesssQuery
{
    public class GetBusinesssQuery : IRequest<GetBusinesssQueryDto>
    {
        public BusinessTypeEnum? BusinessType { get; set; }
    }

    public class GetBusinessQueryHandler : IRequestHandler<GetBusinesssQuery, GetBusinesssQueryDto>
    {
        private readonly IBusinessUnitOfWork _businessUnitOfWork;

        public GetBusinessQueryHandler(IBusinessUnitOfWork businessUnitOfWork)
        {
            _businessUnitOfWork = businessUnitOfWork;
        }

        public async Task<GetBusinesssQueryDto> Handle(GetBusinesssQuery query, CancellationToken cancellationToken)
        {
            var response = new GetBusinesssQueryDto();

            var businesss = await _businessUnitOfWork.BusinessDataLayer.GetBusinesses();

            if (businesss.Count == 0)
            {
                return response;
            }

            response.Businesses = businesss
                .Where(b => !query.BusinessType.HasValue || (int)query.BusinessType.Value == b.BusinessTypeId)
                .Select(b => new BusinessListItemDTO()
                {
                    Country = b.Country.Name,
                    Name = b.Name,
                    Type = b.Type.Name,
                    BusinessId = b.BusinessId
                })
                .ToList();

            return response;
        }
    }
}
