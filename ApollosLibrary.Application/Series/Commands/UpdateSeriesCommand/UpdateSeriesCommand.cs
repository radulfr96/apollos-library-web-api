using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Series.Commands.UpdateSeriesCommand
{
    public class UpdateSeriesCommand : IRequest<UpdateSeriesCommandDto>
    {
        public int SeriesId { get; set; }
        public string Name { get; set; }
        public Dictionary<int, int> SeriesOrder = new();
    }

    public class UpdateSeriesCommandHandler : IRequestHandler<UpdateSeriesCommand, UpdateSeriesCommandDto>
    {
        private readonly ISeriesUnitOfWork _seriesUnitOfWork;
        private readonly IBookUnitOfWork _bookUnitOfWork;
        private readonly IUserService _userService;
        private readonly IDateTimeService _dateTimeService;

        public UpdateSeriesCommandHandler(
            ISeriesUnitOfWork seriesUnitOfWork
            , IBookUnitOfWork bookUnitOfWork
            , IUserService userService
            , IDateTimeService dateTimeService)
        {
            _seriesUnitOfWork = seriesUnitOfWork;
            _bookUnitOfWork = bookUnitOfWork;
            _userService = userService;
            _dateTimeService = dateTimeService;
        }

        public async Task<UpdateSeriesCommandDto> Handle(UpdateSeriesCommand command, CancellationToken cancellationToken)
        {
            var response = new UpdateSeriesCommandDto();

            var series = await _seriesUnitOfWork.SeriesDataLayer.GetSeries(command.SeriesId);

            if (series == null)
            {
                throw new SeriesNotFoundException($"Unable to find series with id [{command.SeriesId}] for update");
            }

            series.Name = command.Name;

            await _seriesUnitOfWork.SeriesDataLayer.DeleteSeriesOrder(command.SeriesId);

            foreach (var book in command.SeriesOrder)
            {
                var bookEntity = _bookUnitOfWork.BookDataLayer.GetBook(book.Key);

                if (bookEntity == null)
                {
                    throw new BookNotFoundException($"Unable to find book with id [{book.Key}]");
                }

                series.SeriesOrders.Add(new Domain.SeriesOrder()
                {
                    BookId = book.Key,
                    Number = book.Value,
                });
            }

            series.ModifiedDate = _dateTimeService.Now;
            series.ModifiedBy = _userService.GetUserId();

            await _seriesUnitOfWork.Save();

            return response;
        }
    }
}
