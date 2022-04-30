using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Domain;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Library.Commands.AddLibraryEntryCommand
{
    public class AddLibraryEntryCommand : IRequest<AddLibraryEntryCommandDto>
    {
        public int BookId { get; set; }
        public int LibraryId { get; set; }
        public int Quantity { get; set; }
    }

    public class AddLibraryEntryCommandHandler : IRequestHandler<AddLibraryEntryCommand, AddLibraryEntryCommandDto>
    {
        private readonly ILibraryUnitOfWork _libraryUnitOfWork;
        private readonly IBookUnitOfWork _bookUnitOfWork;
        private readonly IUserService _userService;

        public AddLibraryEntryCommandHandler(
            ILibraryUnitOfWork libraryUnitOfWork
            , IBookUnitOfWork bookUnitOfWork
            , IUserService userService
            )
        {
            _bookUnitOfWork = bookUnitOfWork;
            _libraryUnitOfWork = libraryUnitOfWork;
            _userService = userService;
        }

        public async Task<AddLibraryEntryCommandDto> Handle(AddLibraryEntryCommand command, CancellationToken cancellationToken)
        {
            var response = new AddLibraryEntryCommandDto();

            var library = await _libraryUnitOfWork.LibraryDataLayer.GetLibrary(command.LibraryId);

            if (library == null)
            {
                throw new LibraryNotFoundException($"Unable to find library with id of [{command.LibraryId}]");
            }

            if (library.UserId != _userService.GetUserId())
            {
                throw new UserCannotModifyLibraryException($"User does not have permission to modify library with id of [{command.LibraryId}]");
            }

            var book = await _bookUnitOfWork.BookDataLayer.GetBook(command.BookId);

            if (book == null)
            {
                throw new BookNotFoundException($"Unable to find book with id of [{command.BookId}]");
            }

            var entry = new LibraryEntry()
            {
                BookId = command.BookId,
                LibraryId = command.LibraryId,
                Quantity = command.Quantity,
            };

            await _libraryUnitOfWork.LibraryDataLayer.AddLibraryEntry(entry);
            await _libraryUnitOfWork.Save();

            response.LibraryEntryId = entry.EntryId;

            return response;
        }
    }
}
