using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Domain;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Series.Commands.AddSeriesCommand
{
    public class AddSeriesCommand : IRequest<AddSeriesCommandDto>
    {
        public string Name { get; set; }
    }

    public class AddSeriesCommandHandler : IRequestHandler<AddSeriesCommand, AddSeriesCommandDto>
    {
        private readonly ISeriesUnitOfWork _seriesUnitOfWork;
        private readonly IUserService _userService;
        private readonly IDateTimeService _dateTimeService;

        public AddSeriesCommandHandler(
            ISeriesUnitOfWork seriesUnitOfWork
            , IUserService userService
            , IDateTimeService dateTimeService)
        {
            _seriesUnitOfWork = seriesUnitOfWork;
            _userService = userService;
            _dateTimeService = dateTimeService;
        }

        public async Task<AddSeriesCommandDto> Handle(AddSeriesCommand command, CancellationToken cancellationToken)
        {
            var response = new AddSeriesCommandDto();

            var series = new Domain.Series()
            {
                CreatedBy = _userService.GetUserId(),
                CreatedDate = _dateTimeService.Now,
                Name = command.Name,
            };

            await _seriesUnitOfWork.SeriesDataLayer.AddSeries(series);
            await _seriesUnitOfWork.Begin();
            await _seriesUnitOfWork.Save();

            var record = new SeriesRecord()
            {
                CreatedBy = series.CreatedBy,
                CreatedDate = series.CreatedDate,
                Name = series.Name,
                IsDeleted = false,
                SeriesId = series.SeriesId,
            };

            await _seriesUnitOfWork.SeriesDataLayer.AddSeriesRecord(record);

            await _seriesUnitOfWork.Save();

            series.VersionId = record.SeriesRecordId;

            await _seriesUnitOfWork.Save();
            await _seriesUnitOfWork.Commit();

            response.SeriesId = series.SeriesId;

            return response;
        }
    }
}
