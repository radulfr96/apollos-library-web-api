using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Series.Queries.GetSeriesQuery
{
    public class GetSeriesQuery : IRequest<GetSeriesQueryDto>
    {
        public int SeriesId { get; set; }
    }

    public class GetSeriesQueryHandler : IRequestHandler<GetSeriesQuery, GetSeriesQueryDto>
    {
        private readonly ISeriesUnitOfWork _seriesUnitOfWork;

        public GetSeriesQueryHandler(ISeriesUnitOfWork seriesUnitOfWork)
        {
            _seriesUnitOfWork = seriesUnitOfWork;
        }

        public async Task<GetSeriesQueryDto> Handle(GetSeriesQuery query, CancellationToken cancellationToken)
        {
            var response = new GetSeriesQueryDto();

            var series = await _seriesUnitOfWork.SeriesDataLayer.GetSeries(query.SeriesId);

            if (series == null)
            {
                throw new SeriesNotFoundException($"Unable to find series with id {query.SeriesId}");
            }

            response.SeriesId = series.SeriesId;
            response.Name = series.Name;
            return response;
        }
    }
}
