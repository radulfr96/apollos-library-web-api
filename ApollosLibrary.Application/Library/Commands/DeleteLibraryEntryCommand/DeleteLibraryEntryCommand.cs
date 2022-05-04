using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Library.Commands.DeleteLibraryEntryCommand
{
    public class DeleteLibraryEntryCommand : IRequest<DeleteLibraryEntryCommandDto>
    {
        public int LibraryEntryId { get; set; }
    }

    public class DeleteLibraryEntryCommandHandler : IRequestHandler<DeleteLibraryEntryCommand, DeleteLibraryEntryCommandDto>
    {
        private readonly ILibraryUnitOfWork _libraryUnitOfWork;
        private readonly IUserService _userService;

        public DeleteLibraryEntryCommandHandler(ILibraryUnitOfWork libraryUnitOfWork, IUserService userService)
        {
            _libraryUnitOfWork = libraryUnitOfWork;
            _userService = userService;
        }

        public async Task<DeleteLibraryEntryCommandDto> Handle(DeleteLibraryEntryCommand command, CancellationToken cancellationToken)
        {
            var response = new DeleteLibraryEntryCommandDto();

            var libraryId = await _libraryUnitOfWork.LibraryDataLayer.GetLibraryIdByUserId(_userService.GetUserId());

            if (libraryId == null)
            {
                throw new LibraryNotFoundException($"Unable to find library for user]");
            }

            var entry = await _libraryUnitOfWork.LibraryDataLayer.GetLibraryEntry(command.LibraryEntryId);

            if (entry == null)
            {
                throw new LibraryEntryNotFoundException($"Unable to find library entry with id [{command.LibraryEntryId}]");
            }
            else if (entry.LibraryId != libraryId)
            {
                throw new UserCannotModifyLibraryException("User does not have permission to modify this library");
            }

            await _libraryUnitOfWork.LibraryDataLayer.DeleteLibraryEntry(command.LibraryEntryId);
            await _libraryUnitOfWork.Save();

            return response;
        }
    }
}
