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

namespace ApollosLibrary.Application.Business.Commands.UpdateBusinessCommand
{
    public class UpdateBusinessCommand : IRequest<UpdateBusinessCommandDto>
    {
        public int BusinessId { get; set; }
        public int BusinessTypeId { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string State { get; set; }
        public string CountryID { get; set; }
    }

    public class UpdateBusinessCommandHandler : IRequestHandler<UpdateBusinessCommand, UpdateBusinessCommandDto>
    {
        private readonly IBusinessUnitOfWork _businessUnitOfWork;
        private readonly IReferenceUnitOfWork _referenceUnitOfWork;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserService _userService;

        public UpdateBusinessCommandHandler(
            IBusinessUnitOfWork BusinessUnitOfWork
            , IReferenceUnitOfWork referenceUnitOfWork
            , IDateTimeService dateTimeService
            , IUserService userService
            )
        {
            _businessUnitOfWork = BusinessUnitOfWork;
            _referenceUnitOfWork = referenceUnitOfWork;
            _dateTimeService = dateTimeService;
            _userService = userService;
        }

        public async Task<UpdateBusinessCommandDto> Handle(UpdateBusinessCommand command, CancellationToken cancellationToken)
        {
            var response = new UpdateBusinessCommandDto();

            var business = await _businessUnitOfWork.BusinessDataLayer.GetBusiness(command.BusinessId);

            if (business == null)
            {
                throw new BusinessNotFoundException($"Unable to find Business with id {command.BusinessId}");
            }

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

            var record = new Domain.BusinessRecord()
            {
                BusinessId = command.BusinessId,
                BusinessTypeId = command.BusinessTypeId,
                City = command.City,
                CountryId = command.CountryID,
                CreatedBy = _userService.GetUserId(),
                CreatedDate = _dateTimeService.Now,
                IsDeleted = false,
                Name = command.Name,
                Postcode = command.Postcode,
                State = command.State,
                StreetAddress = command.StreetAddress,
                Website = command.Website,
            };

            await _businessUnitOfWork.BusinessDataLayer.AddBusinessRecord(record);
            await _businessUnitOfWork.Begin();
            await _businessUnitOfWork.Save();

            business.City = command.City;
            business.CountryId = command.CountryID;
            business.BusinessTypeId = command.BusinessTypeId;
            business.IsDeleted = false;
            business.Name = command.Name;
            business.Postcode = command.Postcode;
            business.State = command.State;
            business.StreetAddress = command.StreetAddress;
            business.Website = command.Website;
            business.CreatedDate = _dateTimeService.Now;
            business.VersionId = record.BusinessRecordId;

            await _businessUnitOfWork.Save();
            await _businessUnitOfWork.Commit();

            return response;
        }
    }
}
