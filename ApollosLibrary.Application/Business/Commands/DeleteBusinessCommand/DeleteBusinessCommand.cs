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
        private readonly IBusinessUnitOfWork _BusinessUnitOfWork;

        public DeleteBusinessCommandHandler(IBusinessUnitOfWork BusinessUnitOfWork)
        {
            _BusinessUnitOfWork = BusinessUnitOfWork;
        }

        public async Task<DeleteBusinessCommandDto> Handle(DeleteBusinessCommand command, CancellationToken cancellationToken)
        {
            var response = new DeleteBusinessCommandDto();

            var Business = await _BusinessUnitOfWork.BusinessDataLayer.GetBusiness(command.BusinessId);

            if (Business == null)
            {
                throw new BusinessNotFoundException($"Unable to find Business with id {command.BusinessId}");
            }

            Business.IsDeleted = true;
            Business.Name = "Deleted";
            Business.City = "Deleted";
            Business.Postcode = "0000";
            Business.State = "Deleted";
            Business.StreetAddress = "Deleted";
            Business.Website = "";
            Business.CountryId = "AU";

            await _BusinessUnitOfWork.Save();

            return response;
        }
    }
}
