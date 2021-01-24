using MediatR;
using MyLibrary.Application.Interfaces;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Publisher.Commands.UpdatePublisherCommand
{
    public class UpdatePublisherCommand : IRequest<UpdatePublisherCommandDto>
    {
        public int PublisherID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "You must provide a publisher name")]
        public string Name { get; set; }
        public string Website { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string State { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must select a country")]
        public string CountryID { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (string.IsNullOrEmpty(Website))
            {
                if (string.IsNullOrEmpty(StreetAddress)
                    && string.IsNullOrEmpty(City)
                    && string.IsNullOrEmpty(Postcode)
                    && string.IsNullOrEmpty(State))
                {
                    results.Add(new ValidationResult("You must provide the publisher address or website."));
                }
            }

            if ((!string.IsNullOrEmpty(StreetAddress)
                    || !string.IsNullOrEmpty(City)
                    || !string.IsNullOrEmpty(Postcode)
                    || !string.IsNullOrEmpty(State)
                    )
                    &&
                    (
                    string.IsNullOrEmpty(StreetAddress)
                    || string.IsNullOrEmpty(City)
                    || string.IsNullOrEmpty(Postcode))
                    )
            {
                results.Add(new ValidationResult("You must provide a full address."));
            }

            return results;
        }
    }

    public class UpdatePublisherCommandHandler : IRequestHandler<UpdatePublisherCommand, UpdatePublisherCommandDto>
    {
        private IPublisherUnitOfWork _publisherUnitOfWork;
        private IUserService _userService;
        private IDateTimeService _dateTimeService;

        public UpdatePublisherCommandHandler(IPublisherUnitOfWork publisherUnitOfWork, IUserService userService, IDateTimeService dateTimeService)
        {
            _publisherUnitOfWork = publisherUnitOfWork;
            _userService = userService;
            _dateTimeService = dateTimeService;
        }

        public async Task<UpdatePublisherCommandDto> Handle(UpdatePublisherCommand command, CancellationToken cancellationToken)
        {
            var response = new UpdatePublisherCommandDto();

            var publisher = await _publisherUnitOfWork.PublisherDataLayer.GetPublisher(command.PublisherID);

            if (publisher == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }

            publisher.City = command.City;
            publisher.CountryId = command.CountryID;
            publisher.IsDeleted = false;
            publisher.ModifiedBy = _userService.GetUserId();
            publisher.ModifiedDate = _dateTimeService.Now;
            publisher.Name = command.Name;
            publisher.Postcode = command.Postcode;
            publisher.State = command.State;
            publisher.StreetAddress = command.StreetAddress;
            publisher.Website = command.Website;
            await _publisherUnitOfWork.Save();

            return response;
        }
    }
}
