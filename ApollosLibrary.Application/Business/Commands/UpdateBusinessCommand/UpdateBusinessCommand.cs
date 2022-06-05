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
        private readonly IBusinessUnitOfWork _BusinessUnitOfWork;
        private readonly IReferenceUnitOfWork _referenceUnitOfWork;

        public UpdateBusinessCommandHandler(
            IBusinessUnitOfWork BusinessUnitOfWork
            , IReferenceUnitOfWork referenceUnitOfWork
            )
        {
            _BusinessUnitOfWork = BusinessUnitOfWork;
            _referenceUnitOfWork = referenceUnitOfWork;
        }

        public async Task<UpdateBusinessCommandDto> Handle(UpdateBusinessCommand command, CancellationToken cancellationToken)
        {
            var response = new UpdateBusinessCommandDto();

            var business = await _BusinessUnitOfWork.BusinessDataLayer.GetBusiness(command.BusinessId);

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

            business.City = command.City;
            business.CountryId = command.CountryID;
            business.BusinessTypeId = command.BusinessTypeId;
            business.IsDeleted = false;
            business.Name = command.Name;
            business.Postcode = command.Postcode;
            business.State = command.State;
            business.StreetAddress = command.StreetAddress;
            business.Website = command.Website;
            await _BusinessUnitOfWork.Save();

            return response;
        }
    }
}
