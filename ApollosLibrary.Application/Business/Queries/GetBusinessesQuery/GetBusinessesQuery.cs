using MediatR;
using ApollosLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Business.Queries.GetBusinesssQuery
{
    public class GetBusinesssQuery : IRequest<GetBusinesssQueryDto>
    {
    }

    public class GetBusinessQueryHandler : IRequestHandler<GetBusinesssQuery, GetBusinesssQueryDto>
    {
        private readonly IBusinessUnitOfWork _BusinessUnitOfWork;

        public GetBusinessQueryHandler(IBusinessUnitOfWork BusinessUnitOfWork)
        {
            _BusinessUnitOfWork = BusinessUnitOfWork;
        }

        public async Task<GetBusinesssQueryDto> Handle(GetBusinesssQuery request, CancellationToken cancellationToken)
        {
            var response = new GetBusinesssQueryDto();

            var Businesss = await _BusinessUnitOfWork.BusinessDataLayer.GetBusinesss();

            if (Businesss.Count == 0)
            {
                return response;
            }

            response.Businesss = Businesss.Select(p => new BusinessListItemDTO()
            {
                Country = p.Country.Name,
                Name = p.Name,
                BusinessId = p.BusinessId
            }).ToList();

            return response;
        }
    }
}
