using MediatR;
using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Business.Commands.AddBusinessCommand
{
    public class AddBusinessCommand : IRequest<AddBusinessCommandDto>
    {
        public string Name { get; set; }
        public string Website { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string State { get; set; }
        public string CountryID { get; set; }
        public int BusinessTypeId { get; set; }
    }

    public class AddBusinessCommandHandler : IRequestHandler<AddBusinessCommand, AddBusinessCommandDto>
    {
        private readonly IBusinessUnitOfWork _BusinessUnitOfWork;
        private readonly IReferenceUnitOfWork _referenceUnitOfWork;
        private readonly IUserService _userService;
        private readonly IDateTimeService _dateTimeService;

        public AddBusinessCommandHandler(
            IBusinessUnitOfWork BusinessUnitOfWork
            , IReferenceUnitOfWork referenceUnitOfWork
            , IUserService userService
            , IDateTimeService dateTimeService)
        {
            _BusinessUnitOfWork = BusinessUnitOfWork;
            _referenceUnitOfWork = referenceUnitOfWork;
            _userService = userService;
            _dateTimeService = dateTimeService;
        }

        public async Task<AddBusinessCommandDto> Handle(AddBusinessCommand command, CancellationToken cancellationToken)
        {
            var response = new AddBusinessCommandDto();

            var countries = (await _referenceUnitOfWork.ReferenceDataLayer.GetCountries()).Select(c => c.CountryId).ToList();

            if (!countries.Contains(command.CountryID))
            {
                throw new CountryInvalidValueException($"Unable to find country with code [{command.CountryID}]");
            }

            var businessType = (await _referenceUnitOfWork.ReferenceDataLayer.GetBusinessTypes()).Select(c => c.BusinessTypeId).ToList();

            if (businessType == null)
            {
                throw new BusinessTypeNotFoundException($"Unable to find BusinessType with code [{command.BusinessTypeId}]");
            }

            var Business = new Domain.Business()
            {
                CreatedBy = _userService.GetUserId(),
                CreatedDate = _dateTimeService.Now,
                BusinessTypeId = command.BusinessTypeId,
                City = command.City,
                CountryId = command.CountryID,
                IsDeleted = false,
                Postcode = command.Postcode,
                State = command.State,
                StreetAddress = command.StreetAddress,
                Website = command.Website,
                Name = command.Name,
            };

            await _BusinessUnitOfWork.BusinessDataLayer.AddBusiness(Business);
            await _BusinessUnitOfWork.Save();

            response.BusinessId = Business.BusinessId;

            return response;
        }
    }
}
