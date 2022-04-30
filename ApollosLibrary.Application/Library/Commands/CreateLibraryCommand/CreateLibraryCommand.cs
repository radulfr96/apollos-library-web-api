using ApollosLibrary.Application.Interfaces;
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
    public class CreateLibraryCommand : IRequest<CreateLibraryCommandDto> { }

    public class CreateLibraryCommandHandler : IRequestHandler<CreateLibraryCommand, CreateLibraryCommandDto>
    {
        private readonly ILibraryUnitOfWork _libraryUnitOfWork;
        private readonly IUserService _userService;

        public CreateLibraryCommandHandler(ILibraryUnitOfWork libraryUnitOfWork, IUserService userService)
        {
            _libraryUnitOfWork = libraryUnitOfWork;
            _userService = userService;
        }

        public async Task<CreateLibraryCommandDto> Handle(CreateLibraryCommand command, CancellationToken cancellationToken)
        {
            var library = new Domain.Library()
            {
                UserId = _userService.GetUserId(),
            };

            await _libraryUnitOfWork.LibraryDataLayer.AddLibrary(library);
            await _libraryUnitOfWork.Save();

            return new CreateLibraryCommandDto()
            {
                LibraryId = library.LibraryId,
            };
        }
    }
}
