using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Series.Queries.GetMultiSeriesQuery
{
    public class GetMultiSeriesQuery : IRequest<GetMultiSeriesQueryDto>
    {
    }

    public class GetMultiSeriesQueryHandler : IRequestHandler<GetMultiSeriesQuery, GetMultiSeriesQueryDto>
    {
        private readonly ISeriesUnitOfWork _seriesUnitOfWork;

        public GetMultiSeriesQueryHandler(ISeriesUnitOfWork seriesUnitOfWork)
        {
            _seriesUnitOfWork = seriesUnitOfWork;
        }

        public async Task<GetMultiSeriesQueryDto> Handle(GetMultiSeriesQuery request, CancellationToken cancellationToken)
        {
            var response = new GetMultiSeriesQueryDto();

            var series = await _seriesUnitOfWork.SeriesDataLayer.GetMultiSeries();

            if (series.Count == 0)
            {
                return response;
            }

            response.Series = series.Select(p => new SeriesListItemDTO()
            {
                Name = p.Name,
                SeriesId = p.SeriesId
            }).ToList();

            return response;
        }
    }
}
