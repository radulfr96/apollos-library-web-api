using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Series.Queries.GetSeriesRecordQuery
{
    public class GetSeriesRecordQuery : IRequest<GetSeriesRecordQueryDto>
    {
        public int SeriesRecordId { get; set; }
    }

    public class GetSeriesRecordQueryHandler : IRequestHandler<GetSeriesRecordQuery, GetSeriesRecordQueryDto>
    {
        private readonly ISeriesUnitOfWork _seriesUnitOfWork;

        public GetSeriesRecordQueryHandler(ISeriesUnitOfWork bookUnitOfWork)
        {
            _seriesUnitOfWork = bookUnitOfWork;
        }

        public async Task<GetSeriesRecordQueryDto> Handle(GetSeriesRecordQuery request, CancellationToken cancellationToken)
        {
            var businessRecord = await _seriesUnitOfWork.SeriesDataLayer.GetSeriesRecord(request.SeriesRecordId);

            if (businessRecord == null)
            {
                throw new SeriesRecordNotFoundException($"Unable to find series record with id of [{request.SeriesRecordId}]");
            }

            return new GetSeriesRecordQueryDto()
            {
                Name = businessRecord.Name,
                SeriesId = businessRecord.SeriesId,
            };
        }
    }
}
