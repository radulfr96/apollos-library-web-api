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

namespace ApollosLibrary.Application.Library.Commands.UpdateLibraryEntryCommand
{
    public class UpdateLibraryEntryCommand : IRequest<UpdateLibraryEntryCommandDto>
    {
        public int EntryId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateLibraryEntryCommandHandler : IRequestHandler<UpdateLibraryEntryCommand, UpdateLibraryEntryCommandDto>
    {
        private readonly IUserService _userService;
        private readonly ILibraryUnitOfWork _libraryUnitOfWork;

        public UpdateLibraryEntryCommandHandler(IUserService userService, ILibraryUnitOfWork libraryUnitOfWork)
        {
            _userService = userService;
            _libraryUnitOfWork = libraryUnitOfWork;
        }

        public async Task<UpdateLibraryEntryCommandDto> Handle(UpdateLibraryEntryCommand command, CancellationToken cancellationToken)
        {
            var response = new UpdateLibraryEntryCommandDto();

            var entry = await _libraryUnitOfWork.LibraryDataLayer.GetLibraryEntry(command.EntryId);

            if (entry == null)
            {
                throw new LibraryEntryNotFoundException($"Unable to find library entry with id [{command.EntryId}]");
            }

            if (entry.Library.UserId != _userService.GetUserId())
            {
                throw new UserCannotModifyLibraryException($"User does not have permission to change library with id [{entry.LibraryId}]");
            }

            entry.Quantity = command.Quantity;
            await _libraryUnitOfWork.Save();

            return response;
        }
    }
}
