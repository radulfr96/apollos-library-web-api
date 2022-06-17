using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Business.Queries.GetBusinessRecordQuery
{
    public class GetBusinessRecordQuery : IRequest<GetBusinessRecordQueryDto>
    {
        public int BusinessRecordId { get; set; }
    }

    public class GetBusinessRecordQueryHandler : IRequestHandler<GetBusinessRecordQuery, GetBusinessRecordQueryDto>
    {
        private readonly IBusinessUnitOfWork _businessUnitOfWork;
         
        public GetBusinessRecordQueryHandler(IBusinessUnitOfWork bookUnitOfWork)
        {
            _businessUnitOfWork = bookUnitOfWork;
        }

        public async Task<GetBusinessRecordQueryDto> Handle(GetBusinessRecordQuery request, CancellationToken cancellationToken)
        {
            var businessRecord = await _businessUnitOfWork.BusinessDataLayer.GetBusinessRecord(request.BusinessRecordId);

            if (businessRecord == null)
            {
                throw new BusinessRecordNotFoundException($"Unable to find business record with id of [{request.BusinessRecordId}]");
            }

            return new GetBusinessRecordQueryDto()
            {
                BusinessId = businessRecord.BusinessId,
                City = businessRecord.City,
                Name = businessRecord.Name,
                Postcode = businessRecord.Postcode,
                State = businessRecord.State,
                StreetAddress = businessRecord.StreetAddress,
                Website = businessRecord.Website,
            };
        }
    }
}
