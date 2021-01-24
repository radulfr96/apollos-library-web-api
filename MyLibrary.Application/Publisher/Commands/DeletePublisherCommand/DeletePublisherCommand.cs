using MediatR;
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
        public int DeleteId { get; set; }
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

            var publisher = await _publisherUnitOfWork.PublisherDataLayer.GetPublisher(command.DeleteId);

            if (publisher == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
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
