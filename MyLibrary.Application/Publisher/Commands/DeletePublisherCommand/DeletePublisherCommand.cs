using MediatR;
using MyLibrary.Application.Common.Exceptions;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Publisher.Commands.DeletePublisherCommand
{
    public class DeletePublisherCommand : IRequest<DeletePublisherCommandDto>
    {
        public int PubisherId { get; set; }
    }

    public class DeletePublisherCommandHandler : IRequestHandler<DeletePublisherCommand, DeletePublisherCommandDto>
    {
        private readonly IPublisherUnitOfWork _publisherUnitOfWork;

        public DeletePublisherCommandHandler(IPublisherUnitOfWork publisherUnitOfWork)
        {
            _publisherUnitOfWork = publisherUnitOfWork;
        }

        public async Task<DeletePublisherCommandDto> Handle(DeletePublisherCommand command, CancellationToken cancellationToken)
        {
            var response = new DeletePublisherCommandDto();

            var publisher = await _publisherUnitOfWork.PublisherDataLayer.GetPublisher(command.PubisherId);

            if (publisher == null)
            {
                throw new PublisherNotFoundException($"Unable to find publisher with id {command.PubisherId}");
            }

            publisher.IsDeleted = true;
            publisher.Name = "Deleted";
            publisher.City = "Deleted";
            publisher.Postcode = "0000";
            publisher.State = "Deleted";
            publisher.StreetAddress = "Deleted";
            publisher.Website = "";
            publisher.CountryId = "AU";

            await _publisherUnitOfWork.Save();

            return response;
        }
    }
}
