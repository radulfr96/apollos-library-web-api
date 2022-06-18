using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Series.Commands.DeleteSeriesCommand
{
    public class DeleteSeriesCommand : IRequest<DeleteSeriesCommandDto>
    {
        public int SeriesId { get; set; }
    }

    public class DeleteSeriesCommandHandler : IRequestHandler<DeleteSeriesCommand, DeleteSeriesCommandDto>
    {
        private readonly ISeriesUnitOfWork _seriesUnitOfWork;
        public DeleteSeriesCommandHandler(ISeriesUnitOfWork seriesUnitOfWork)
        {
            _seriesUnitOfWork = seriesUnitOfWork;
        }

        public async Task<DeleteSeriesCommandDto> Handle(DeleteSeriesCommand command, CancellationToken cancellationToken)
        {
            var response = new DeleteSeriesCommandDto();

            var series = await _seriesUnitOfWork.SeriesDataLayer.GetSeries(command.SeriesId);

            if (series == null)
            {
                throw new SeriesNotFoundException($"Unable to find series with id {command.SeriesId}");
            }

            var record = new Domain.SeriesRecord()
            {
                CreatedBy = series.CreatedBy,
                CreatedDate = series.CreatedDate,
                Name = series.Name,
                IsDeleted = true,
                SeriesId = series.SeriesId,
            };

            await _seriesUnitOfWork.SeriesDataLayer.AddSeriesRecord(record);

            await _seriesUnitOfWork.Begin();
            await _seriesUnitOfWork.Save();

            series.IsDeleted = true;
            series.VersionId = record.SeriesRecordId;
            series.Books = new List<Domain.Book>();

            await _seriesUnitOfWork.Save();
            await _seriesUnitOfWork.Commit();

            return response;
        }
    }
}
