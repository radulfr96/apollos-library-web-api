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
        public Dictionary<int, int> SeriesOrder = new();
    }

    public class AddSeriesCommandHandler : IRequestHandler<AddSeriesCommand, AddSeriesCommandDto>
    {
        private readonly ISeriesUnitOfWork _seriesUnitOfWork;
        private readonly IBookUnitOfWork _bookUnitOfWork;
        private readonly IUserService _userService;
        private readonly IDateTimeService _dateTimeService;

        public AddSeriesCommandHandler(
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

        public async Task<AddSeriesCommandDto> Handle(AddSeriesCommand command, CancellationToken cancellationToken)
        {
            var response = new AddSeriesCommandDto();

            foreach (var book in command.SeriesOrder)
            {
                var bookEntity = await _bookUnitOfWork.BookDataLayer.GetBook(book.Key);

                if (bookEntity == null)
                {
                    throw new BookNotFoundException($"Unable to find book with id [{book.Key}]");
                }
            }

            var series = new Domain.Series()
            {
                CreatedBy = _userService.GetUserId(),
                CreatedDate = _dateTimeService.Now,
                Name = command.Name,
                SeriesOrders = command.SeriesOrder.Select(b => new SeriesOrder()
                {
                    BookId = b.Key,
                    Number = b.Value,
                }).ToList(),
            };

            await _seriesUnitOfWork.SeriesDataLayer.AddSeries(series);
            await _seriesUnitOfWork.Save();

            response.SeriesId = series.SeriesId;

            return response;
        }
    }
}
