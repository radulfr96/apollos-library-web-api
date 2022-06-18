using MediatR;
using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Business.Commands.DeleteBusinessCommand
{
    public class DeleteBusinessCommand : IRequest<DeleteBusinessCommandDto>
    {
        public int BusinessId { get; set; }
    }

    public class DeleteBusinessCommandHandler : IRequestHandler<DeleteBusinessCommand, DeleteBusinessCommandDto>
    {
        private readonly IBusinessUnitOfWork _businessUnitOfWork;

        public DeleteBusinessCommandHandler(IBusinessUnitOfWork businessUnitOfWork)
        {
            _businessUnitOfWork = businessUnitOfWork;
        }

        public async Task<DeleteBusinessCommandDto> Handle(DeleteBusinessCommand command, CancellationToken cancellationToken)
        {
            var response = new DeleteBusinessCommandDto();

            var business = await _businessUnitOfWork.BusinessDataLayer.GetBusiness(command.BusinessId);

            if (business == null)
            {
                throw new BusinessNotFoundException($"Unable to find Business with id {command.BusinessId}");
            }

            var record = new Domain.BusinessRecord()
            {
                BusinessId = business.BusinessId,
                BusinessTypeId = business.BusinessTypeId,
                City = business.City,
                CountryId = business.CountryId,
                CreatedBy = business.CreatedBy,
                CreatedDate = business.CreatedDate,
                IsDeleted = true,
                Name = business.Name,
                Postcode = business.Postcode,
                State = business.State,
                StreetAddress = business.StreetAddress,
                Website = business.Website,
            };

            await _businessUnitOfWork.BusinessDataLayer.AddBusinessRecord(record);
            await _businessUnitOfWork.Begin();
            await _businessUnitOfWork.Save();

            business.IsDeleted = true;
            business.Name = "Deleted";
            business.City = "Deleted";
            business.Postcode = "0000";
            business.State = "Deleted";
            business.StreetAddress = "Deleted";
            business.Website = "";
            business.CountryId = "AU";
            business.VersionId = record.BusinessRecordId;

            await _businessUnitOfWork.Save();
            await _businessUnitOfWork.Commit();

            return response;
        }
    }
}
