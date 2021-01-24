using MediatR;
using MyLibrary.Application.Interfaces;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Publisher.Commands.AddPublisherCommand
{
    public class AddPublisherCommand : IRequest<AddPublisherCommandDto>
    {
        public string Name { get; set; }
        public string Website { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string State { get; set; }
        public string CountryID { get; set; }
    }

    public class AddPublisherCommandHandler : IRequestHandler<AddPublisherCommand, AddPublisherCommandDto>
    {
        private readonly IPublisherUnitOfWork _publisherUnitOfWork;
        private readonly IUserService _userService;
        private readonly IDateTimeService _dateTimeService;

        public AddPublisherCommandHandler(IPublisherUnitOfWork publisherUnitOfWork, IUserService userService, IDateTimeService dateTimeService)
        {
            _publisherUnitOfWork = publisherUnitOfWork;
            _userService = userService;
            _dateTimeService = dateTimeService;
        }

        public async Task<AddPublisherCommandDto> Handle(AddPublisherCommand command, CancellationToken cancellationToken)
        {
            var response = new AddPublisherCommandDto();

            var publisher = new Persistence.Model.Publisher()
            {
                CreatedBy = _userService.GetUserId(),
                CreatedDate = _dateTimeService.Now,
                City = command.City,
                CountryId = command.CountryID,
                IsDeleted = false,
                Postcode = command.Postcode,
                State = command.State,
                StreetAddress = command.StreetAddress,
                Website = command.Website,
                Name = command.Name,
            };

            await _publisherUnitOfWork.PublisherDataLayer.AddPublisher(publisher);
            await _publisherUnitOfWork.Save();

            return response;
        }
    }
}
