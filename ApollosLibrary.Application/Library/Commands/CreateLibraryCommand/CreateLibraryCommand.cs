using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Library.Commands.CreateLibraryCommand
{
    public class CreateLibraryCommand : IRequest<CreateLibraryCommandDto>
    {
        public Guid UserId { get; set; }
    }

    public class CreateLibraryCommandHandler : IRequestHandler<CreateLibraryCommand, CreateLibraryCommandDto>
    {
        public readonly ILibraryUnitOfWork _libraryUnitOfWork;

        public CreateLibraryCommandHandler(ILibraryUnitOfWork libraryUnitOfWork)
        {
            _libraryUnitOfWork = libraryUnitOfWork;
        }

        public async Task<CreateLibraryCommandDto> Handle(CreateLibraryCommand command, CancellationToken cancellationToken)
        {
            var library = new Domain.Library()
            {
                UserId = command.UserId,
            };

            await _libraryUnitOfWork.LibraryDataLayer.AddLibrary(library);

            return new CreateLibraryCommandDto()
            {
                LibraryId = library.LibraryId,
            };
        }
    }
}
